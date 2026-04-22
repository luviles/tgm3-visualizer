using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Interfaces;
using Tgm3Visualizer.Core.Models;

namespace Tgm3Visualizer.Core.Processors;

/// <summary>
/// Processes game state for Shirase mode (20G time attack)
/// </summary>
public class ShiraseModeProcessor : GameStateProcessorBase
{
    private const int MaxStaffRollFrames = 3265; // 3265f playable time (excludes 360f fanfare)
    private int[] _sectionCompletionTimes = new int[13]; // 0-12 (1300 levels in Shirase)
    private int[] _sectionTimes = new int[13];
    private bool[] _regrets = new bool[13];
    private int _previousLevel;
    private int _previousSection;

    // Session-tracked state
    private bool _staffRollLatch;
    private bool _level500Checked;
    private bool _level1000Checked;
    private bool _level500Exceeded;
    private bool _level1000Exceeded;

    /// <summary>
    /// Max garbage quota thresholds per level section for Rising Garbage mode.
    /// When GarbageQuota reaches this value and next block placed without line clear,
    /// a garbage line is generated and quota resets to 0.
    /// Indexed by section (5-9 = levels 500-999).
    /// Section 5 (500-599): 20, Section 6 (600-699): 18, Section 7 (700-799): 10,
    /// Section 8 (800-899): 9, Section 9 (900-999): 8
    /// </summary>
    public static readonly int[] ShiraseGarbageQuotaMax = { 20, 18, 10, 9, 8 };

    public override ProcessedGameState Process(GameState currentState)
    {
        var result = new ProcessedGameState();

        #region 1. Common Fields + Shirase-specific Input

        CopyCommonFields(currentState, result);

        #endregion

        #region 2. Reset Detection (TimeFrames == 0)

        if (currentState.TimeFrames == 0)
        {
            result.StatusCardTitle = "NORMAL\nPLAY";
            result.StatusCardSubtitle = "";
            result.ShowStaffRollTime = false;

            // Compute time limit displays even on reset (ViewModel needs them)
            result.Level500TimeLimitDisplay = result.ControlMode == "WORLD"
                ? SectionTimeCalculator.FormatTime(SectionConstants.ShiraseWorldLevel500Limit)
                : SectionTimeCalculator.FormatTime(SectionConstants.ShiraseClassicLevel500Limit);
            result.Level1000TimeLimitDisplay = result.ControlMode == "WORLD"
                ? SectionTimeCalculator.FormatTime(SectionConstants.ShiraseWorldLevel1000Limit)
                : SectionTimeCalculator.FormatTime(SectionConstants.ShiraseClassicLevel1000Limit);

            Reset();
            return result;
        }

        #endregion

        #region 3. Section Tracking

        var currentSection = currentState.Section;

        // Detect section boundary crossed
        if (_previousSection < currentSection && _previousLevel > 0)
        {
            _sectionCompletionTimes[_previousSection] = currentState.TimeFrames;

            var previousTotal = _previousSection > 0
                ? _sectionCompletionTimes[_previousSection - 1]
                : 0;
            var sectionTime = currentState.TimeFrames - previousTotal;
            _sectionTimes[_previousSection] = sectionTime;

            // Check for Regret
            var isRegret = RegretCalculator.IsShiraseRegret(sectionTime, _previousSection);
            _regrets[_previousSection] = isRegret;
        }

        // Copy section times and regret status
        result.SectionTimes = (int[])_sectionTimes.Clone();
        result.Regrets = (bool[])_regrets.Clone();

        #endregion

        #region 4. Current Section Time + Deadline + Level Time Limit

        // Calculate current section time
        var prevSectionTotal = currentSection > 0 && _sectionCompletionTimes[currentSection - 1] > 0
            ? _sectionCompletionTimes[currentSection - 1]
            : 0;
        result.CurrentSectionTime = currentState.TimeFrames - prevSectionTotal;

        // Calculate Regret deadline for current section
        if (currentSection < 13)
        {
            var regretThreshold = SectionConstants.GetShiraseRegretThresholdFrames(currentSection);
            result.RegretDeadline = prevSectionTotal + regretThreshold;
        }

        // Level Time Limit check (first entry into level 500/1000)
        if (!_level500Checked && currentState.Level >= 500 && _previousLevel < 500)
        {
            var limit = result.ControlMode == "WORLD"
                ? SectionConstants.ShiraseWorldLevel500Limit
                : SectionConstants.ShiraseClassicLevel500Limit;
            _level500Exceeded = currentState.TimeFrames > limit;
            _level500Checked = true;
        }

        if (!_level1000Checked && currentState.Level >= 1000 && _previousLevel < 1000)
        {
            var limit = result.ControlMode == "WORLD"
                ? SectionConstants.ShiraseWorldLevel1000Limit
                : SectionConstants.ShiraseClassicLevel1000Limit;
            _level1000Exceeded = currentState.TimeFrames > limit;
            _level1000Checked = true;
        }

        result.Level500TimeLimitExceeded = _level500Exceeded;
        result.Level1000TimeLimitExceeded = _level1000Exceeded;

        _previousLevel = currentState.Level;
        _previousSection = currentSection;

        // Speed Level = Level for Shirase mode (no Cool bonus)
        result.SpeedLevel = currentState.Level;
        result.MaxLockFrame = LockFrameConstants.GetMaxLockFrame(
            LockFrameConstants.ShiraseMaxLockFrames, result.SpeedLevel);

        #endregion

        #region 5. Section Time Display Formatting

        ComputeSectionTimeDisplays(result.SectionTimes, 13,
            out var timeDisplays, out var cumulativeDisplays);

        // Real-time display for current section
        if (currentSection >= 0 && currentSection < 13
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
        }

        result.SectionTimeDisplays = timeDisplays;
        result.CumulativeTimeDisplays = cumulativeDisplays;

        #endregion

        #region 6. Shirase Display Formatting

        // Garbage Quota - only relevant during Rising Garbage (levels 500-999)
        if (currentState.Level >= 500 && currentState.Level < 1000)
        {
            int sectionIndex = (currentState.Level / 100) - 5; // 0-4 for sections 5-9
            if (sectionIndex >= 0 && sectionIndex < ShiraseGarbageQuotaMax.Length)
                result.MaxGarbageQuota = ShiraseGarbageQuotaMax[sectionIndex];
        }

        // Combined grade for Shirase: FinalGrade + SectionGradePoints
        result.CombinedGrade = result.FinalGrade + result.SectionGradePoints;

        // Regret deadline display
        result.RegretDeadlineDisplay = result.RegretDeadline > 0
            ? SectionTimeCalculator.FormatTime(result.RegretDeadline)
            : "N/A";

        // Garbage quota display
        result.GarbageQuotaText = result.MaxGarbageQuota > 0
            ? $"{result.GarbageQuota}/{result.MaxGarbageQuota}"
            : "";

        // Level time limit displays
        result.Level500TimeLimitDisplay = result.ControlMode == "WORLD"
            ? SectionTimeCalculator.FormatTime(SectionConstants.ShiraseWorldLevel500Limit)
            : SectionTimeCalculator.FormatTime(SectionConstants.ShiraseClassicLevel500Limit);
        result.Level1000TimeLimitDisplay = result.ControlMode == "WORLD"
            ? SectionTimeCalculator.FormatTime(SectionConstants.ShiraseWorldLevel1000Limit)
            : SectionTimeCalculator.FormatTime(SectionConstants.ShiraseClassicLevel1000Limit);

        #endregion

        #region 7. Status Card

        result.RemainingStaffRollFrames = MaxStaffRollFrames - currentState.StaffRollTime;
        result.IsStaffRollActive = (currentState.GameFlag & 0x01) == 0x01;

        // Latch staff roll state once detected — flag clears before TimeFrames resets
        if (result.IsStaffRollActive)
            _staffRollLatch = true;

        if (_staffRollLatch)
        {
            result.StatusCardTitle = "STAFF ROLL!";
            result.StatusCardSubtitle = result.RemainingStaffRollFrames > 0
                ? SectionTimeCalculator.FormatTime(result.RemainingStaffRollFrames)
                : "CLEAR!!";
            result.ShowStaffRollTime = true;
        }
        else
        {
            result.ShowStaffRollTime = false;
            result.StatusCardSubtitle = "";

            if (currentState.Level >= 1000)
                result.StatusCardTitle = "MONOCHROME\nBLOCK";
            else if (currentState.Level >= 500)
                result.StatusCardTitle = "RISING GARBAGE";
            else
                result.StatusCardTitle = "NORMAL\nPLAY";
        }

        #endregion

        return result;
    }

    public override void Reset()
    {
        _sectionCompletionTimes = new int[13];
        _sectionTimes = new int[13];
        _regrets = new bool[13];
        _previousLevel = 0;
        _previousSection = 0;

        _staffRollLatch = false;
        _level500Checked = false;
        _level1000Checked = false;
        _level500Exceeded = false;
        _level1000Exceeded = false;
    }
}
