using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Interfaces;
using Tgm3Visualizer.Core.Models;

namespace Tgm3Visualizer.Core.Processors;

/// <summary>
/// Processes game state for Master mode (Cool/Regret tracking)
/// </summary>
public class MasterModeProcessor : GameStateProcessorBase
{
    private const int MaxStaffRollFrames = 3265; // 3265f playable time (excludes 360f fanfare)
    private int[] _sectionCompletionTimes = new int[10];
    private int[] _sectionTimes = new int[10];
    private int[] _seventyPercentTimes = new int[10];
    private bool[] _cools = new bool[10];
    private bool[] _regrets = new bool[10];
    private int _previousCoolTime = int.MaxValue;
    private int _previousLevel;
    private int _previousSection;
    private int _previousCheckpointCount;

    // Session-tracked state
    private int _maxCoolThisSession;
    private double _maxStaffRollGradeThisSession;
    private bool _staffRollClearApplied;
    private bool _staffRollLatch;

    public override ProcessedGameState Process(GameState currentState)
    {
        var result = new ProcessedGameState();

        #region 1. Common Fields + Master-specific Input

        CopyCommonFields(currentState, result);

        // Master-specific fields
        result.RemainingStaffRollFrames = MaxStaffRollFrames - currentState.StaffRollTime;

        // Calculate Staff Roll Grade Points based on Invisible/Fading roll
        bool isInvisible = (currentState.FieldStatus & 0x40) == 0x40;
        if (isInvisible)
        {
            result.StaffRollGradePoints = (0.6 * currentState.StaffRollTetrisCount) + (0.1 * currentState.StaffRollLineClear);
        }
        else
        {
            result.StaffRollGradePoints = (0.1 * currentState.StaffRollTetrisCount) + (0.04 * currentState.StaffRollLineClear);
        }

        // StaffRollClearPoints will be calculated later (needs IsStaffRollActive)
        result.StaffRollClearPoints = 0;

        // Convert InternalGrade to display string
        result.InternalGradeDisplay = InternalGradeConverter.ToDisplayGrade(currentState.InternalGrade);

        #endregion

        #region 2. Reset Detection (TimeFrames == 0)

        if (currentState.TimeFrames == 0)
        {
            // Set computed values before returning (ViewModel needs these on reset)
            result.CombinedGrade = result.FinalGrade + result.SectionGradePoints + (int)(result.StaffRollGradePoints + result.StaffRollClearPoints);
            result.IsPromotionExam = (result.MasterExamFlags & 0x02) != 0;
            result.IsDemotionExam = !result.IsPromotionExam && (result.MasterExamFlags & 0x10) != 0;

            if (result.IsDemotionExam)
                result.StatusCardTitle = "DEMOTIONAL\nEXAM";
            else if (result.IsPromotionExam)
                result.StatusCardTitle = "PROMOTIONAL\nEXAM";
            else
                result.StatusCardTitle = "NORMAL\nPLAY";
            result.StatusCardSubtitle = "";
            result.ShowStaffRollTime = false;

            // Grade History (available from memory even on reset)
            result.GradeDisplay = GradeConverter.ToGradeName(result.CombinedGrade);
            var resetGradeHistory = result.ControlMode == "WORLD"
                ? result.WorldMasterGradeHistory : result.ClassicMasterGradeHistory;
            result.SelectedDemotionPoints = result.ControlMode == "WORLD"
                ? result.WorldDemotionPoints : result.ClassicDemotionPoints;
            var resetHistory = GradeHistoryCalculator.Calculate(resetGradeHistory);
            result.GradeHistoryEntries = resetHistory.Entries;
            result.AverageGradeDisplay = resetHistory.AverageGrade;
            result.DemotionProgressDisplay = $"{result.SelectedDemotionPoints}/30";

            Reset();
            return result;
        }

        #endregion

        #region 3. Section Tracking

        var currentSection = currentState.Section;

        // Detect section boundary crossed
        if (_previousSection < currentSection && _previousLevel > 0)
        {
            // Record section completion time
            _sectionCompletionTimes[_previousSection] = currentState.TimeFrames;

            // Calculate section time
            var previousTotal = _previousSection > 0
                ? _sectionCompletionTimes[_previousSection - 1]
                : 0;
            var sectionTime = currentState.TimeFrames - previousTotal;
            _sectionTimes[_previousSection] = sectionTime;

            // Check for Regret
            var isRegret = RegretCalculator.IsRegret(sectionTime, _previousSection);
            _regrets[_previousSection] = isRegret;
        }

        // Detect final section completion (level 999 in Master mode)
        if (currentState.Level == 999 && _previousLevel < 999)
        {
            _sectionCompletionTimes[9] = currentState.TimeFrames;

            var previousTotal = _sectionCompletionTimes[8] > 0 ? _sectionCompletionTimes[8] : 0;
            var sectionTime = currentState.TimeFrames - previousTotal;
            _sectionTimes[9] = sectionTime;

            var isRegret = RegretCalculator.IsRegret(sectionTime, 9);
            _regrets[9] = isRegret;
        }

        // Detect 70% checkpoint (memory-based detection, not available in last section)
        if (currentSection < 9 && _previousCheckpointCount < currentState.SeventyPercentCheckpointCount)
        {
            var previousTotal = currentSection > 0
                ? _sectionCompletionTimes[currentSection - 1]
                : 0;
            var seventyPercentTime = currentState.TimeFrames - previousTotal;

            _seventyPercentTimes[currentSection] = seventyPercentTime;
            _previousCoolTime = seventyPercentTime;
            _previousCheckpointCount = currentState.SeventyPercentCheckpointCount;
        }

        // Detect Section COOL from game memory
        if (currentState.SectionCoolFlag == 1 && !_cools[currentSection])
        {
            _cools[currentSection] = true;
        }

        // Copy accumulated states
        result.SectionTimes = (int[])_sectionTimes.Clone();
        result.SeventyPercentTimes = (int[])_seventyPercentTimes.Clone();
        result.Cools = (bool[])_cools.Clone();
        result.Regrets = (bool[])_regrets.Clone();
        result.TotalCools = _cools.Count(c => c);

        // Speed Level: Level + (Cool Count * 100) for Master mode
        result.SpeedLevel = currentState.Level + (result.TotalCools * 100);
        result.MaxLockFrame = LockFrameConstants.GetMaxLockFrame(
            LockFrameConstants.MasterMaxLockFrames, result.SpeedLevel);

        #endregion

        #region 4. Current Section Time + Deadline

        // Calculate current section time
        var prevSectionTotal = currentSection > 0 && _sectionCompletionTimes[currentSection - 1] > 0
            ? _sectionCompletionTimes[currentSection - 1]
            : 0;
        result.CurrentSectionTime = currentState.TimeFrames - prevSectionTotal;

        // Calculate deadline thresholds for current section
        if (currentSection < 10)
        {
            var regretThreshold = SectionConstants.GetRegretThresholdFrames(currentSection);
            result.RegretDeadline = prevSectionTotal + regretThreshold;

            if (currentSection < 9)
            {
                var absoluteCoolLimit = SectionConstants.GetCoolThresholdFrames(currentSection);
                int effectiveCoolLimit;

                if (_previousCoolTime < int.MaxValue && currentSection > 0 && _cools[currentSection - 1])
                {
                    var graceLimit = _previousCoolTime + 120;
                    effectiveCoolLimit = Math.Min(absoluteCoolLimit, graceLimit);
                }
                else
                {
                    effectiveCoolLimit = absoluteCoolLimit;
                }

                result.CoolDeadline = prevSectionTotal + effectiveCoolLimit;
            }
        }

        _previousLevel = currentState.Level;
        _previousSection = currentSection;

        #endregion

        #region 5. Session Tracking

        // Session tracking: max Cool
        if (result.SectionGradePoints > _maxCoolThisSession)
            _maxCoolThisSession = result.SectionGradePoints;
        result.MaxCoolThisSession = _maxCoolThisSession;

        // Session tracking: max StaffRollGrade with clear bonus
        // 1) Detect clear FIRST (so StaffRollClearPoints is set on same frame)
        if (result.RemainingStaffRollFrames <= 0 && !_staffRollClearApplied && result.IsStaffRollActive)
        {
            _staffRollClearApplied = true;
        }

        // 2) StaffRollClearPoints: non-zero only when clear applied AND staff roll active
        if (_staffRollClearApplied && result.IsStaffRollActive)
        {
            result.StaffRollClearPoints = result.IsInvisible ? 1.6 : 0.5;
        }

        // 3) Track max of (GradePoints + ClearPoints)
        double totalStaffRollPoints = result.StaffRollGradePoints + result.StaffRollClearPoints;
        if (totalStaffRollPoints > _maxStaffRollGradeThisSession)
            _maxStaffRollGradeThisSession = totalStaffRollPoints;

        result.MaxStaffRollGradePoints = _maxStaffRollGradeThisSession;
        result.IsStaffRollCleared = _staffRollClearApplied;

        #endregion

        #region 6. Grade Calculation

        // Combined grade
        result.CombinedGrade = result.FinalGrade + result.SectionGradePoints + (int)(result.StaffRollGradePoints + result.StaffRollClearPoints);

        #endregion

        #region 7. Progress Calculation

        // Progress calculations
        result.AwardedGradePointsProgress = result.AwardedGradePoints / 100.0;
        int maxDecay = DecayTimeMaxCalculator.GetMaxDecayTime(result.InternalGrade);
        result.DecayTimeRemaining = maxDecay - result.DecayTime;
        result.DecayTimeProgress = maxDecay > 0 ? (double)result.DecayTimeRemaining / maxDecay : 0;

        #endregion

        #region 8. Invisible Roll Qualification Check

        result.IsSectionCoolQualified = _maxCoolThisSession >= 9; // Rainbow Brush
        result.IsInternalGradeQualified = result.InternalGrade >= 27; // Lime Brush
        result.IsMaxInternalGrade = result.InternalGrade >= 31; // Rainbow Brush

        #endregion

        #region 9. Exam Status + Status Card

        // Exam status from memory flags (0x4AE32B)
        result.IsPromotionExam = (result.MasterExamFlags & 0x02) != 0;
        result.IsDemotionExam = !result.IsPromotionExam && (result.MasterExamFlags & 0x10) != 0;

        // Status Card
        string remainingTime = result.RemainingStaffRollFrames > 0
            ? SectionTimeCalculator.FormatTime(result.RemainingStaffRollFrames)
            : "CLEAR!!";
        result.IsStaffRollActive = (currentState.GameFlag & 0x01) == 0x01;

        // Latch staff roll state once detected — flag clears before TimeFrames resets
        if (result.IsStaffRollActive)
            _staffRollLatch = true;

        if (_staffRollLatch)
        {
            result.StatusCardTitle = result.IsInvisible ? "INVISIBLE ROLL!!" : "FADING ROLL";
            result.StatusCardSubtitle = remainingTime;
            result.ShowStaffRollTime = true;
        }
        else
        {
            result.ShowStaffRollTime = false;
            result.StatusCardSubtitle = "";
            if (result.IsDemotionExam)
                result.StatusCardTitle = "DEMOTIONAL\nEXAM";
            else if (result.IsPromotionExam)
                result.StatusCardTitle = "PROMOTIONAL\nEXAM";
            else
                result.StatusCardTitle = "NORMAL\nPLAY";
        }

        #endregion

        #region 10. Section Time Display Formatting

        ComputeSectionTimeDisplays(result.SectionTimes, 10,
            out var timeDisplays, out var cumulativeDisplays);

        // 70% time displays
        var seventyPercentDisplays = new string[10];
        for (int i = 0; i < 10; i++)
        {
            if (result.SeventyPercentTimes[i] > 0)
                seventyPercentDisplays[i] = SectionTimeCalculator.FormatTime(result.SeventyPercentTimes[i]);
        }

        // Real-time display for current section
        if (currentSection >= 0 && currentSection < 10
            && result.SectionTimes[currentSection] == 0)
        {
            int cumulative = 0;
            for (int i = 0; i < currentSection; i++)
            {
                if (result.SectionTimes[i] > 0)
                    cumulative += result.SectionTimes[i];
            }
            cumulative += result.CurrentSectionTime;
            timeDisplays[currentSection] = SectionTimeCalculator.FormatTime(result.CurrentSectionTime);
            cumulativeDisplays[currentSection] = SectionTimeCalculator.FormatTime(cumulative);

            // 70% time: counting until x70, then frozen (not available in last section)
            if (currentSection < 9 && result.SeventyPercentTimes[currentSection] == 0)
                seventyPercentDisplays[currentSection] = SectionTimeCalculator.FormatTime(result.CurrentSectionTime);
        }

        result.SectionTimeDisplays = timeDisplays;
        result.CumulativeTimeDisplays = cumulativeDisplays;
        result.SeventyPercentTimeDisplays = seventyPercentDisplays;

        #endregion

        #region 11. Master Display Formatting

        result.GradeDisplay = GradeConverter.ToGradeName(result.CombinedGrade);
        result.InternalGradeText = $"{currentState.InternalGrade} (Grade {result.InternalGradeDisplay})";
        result.StaffRollGradePointsDisplay = GradeHistoryCalculator.FormatGradePoints(result.MaxStaffRollGradePoints);
        result.GameModeDisplay = currentState.GameType.ToString().ToUpper().Replace("_", " ");
        result.DecayTimeDisplay = result.DecayTimeRemaining == 1 ? "1 Frame" : $"{result.DecayTimeRemaining} Frames";
        result.RemainingStaffRollTimeDisplay = result.RemainingStaffRollFrames > 0
            ? SectionTimeCalculator.FormatTime(result.RemainingStaffRollFrames)
            : "CLEAR!!";
        result.CoolDeadlineDisplay = result.CoolDeadline > 0 ? SectionTimeCalculator.FormatTime(result.CoolDeadline) : "N/A";
        result.RegretDeadlineDisplay = result.RegretDeadline > 0 ? SectionTimeCalculator.FormatTime(result.RegretDeadline) : "N/A";

        // Grade History: select by ControlMode
        var gradeHistory = result.ControlMode == "WORLD"
            ? result.WorldMasterGradeHistory : result.ClassicMasterGradeHistory;
        result.SelectedDemotionPoints = result.ControlMode == "WORLD"
            ? result.WorldDemotionPoints : result.ClassicDemotionPoints;

        var history = GradeHistoryCalculator.Calculate(gradeHistory);
        result.GradeHistoryEntries = history.Entries;
        result.AverageGradeDisplay = history.AverageGrade;
        result.DemotionProgressDisplay = $"{result.SelectedDemotionPoints}/30";

        #endregion

        return result;
    }

    public override void Reset()
    {
        _sectionCompletionTimes = new int[10];
        _sectionTimes = new int[10];
        _seventyPercentTimes = new int[10];
        _cools = new bool[10];
        _regrets = new bool[10];
        _previousCoolTime = int.MaxValue;
        _previousLevel = 0;
        _previousSection = 0;
        _previousCheckpointCount = 0;

        _maxCoolThisSession = 0;
        _maxStaffRollGradeThisSession = 0;
        _staffRollClearApplied = false;
        _staffRollLatch = false;
    }
}
