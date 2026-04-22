using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Models;

namespace Tgm3Visualizer.Core.Processors;

/// <summary>
/// Processes game state for Easy mode (Hanabi score, 2-section tracking)
/// </summary>
public class EasyModeProcessor : GameStateProcessorBase
{
    private const int MaxStaffRollFrames = 3265; // 3265f playable time (excludes 360f fanfare)
    private int[] _sectionCompletionTimes = new int[2];
    private int[] _sectionTimes = new int[2];
    private int _previousLevel;
    private int _previousSection;

    // Session-tracked state
    private bool _staffRollLatch;

    public override ProcessedGameState Process(GameState currentState)
    {
        var result = new ProcessedGameState();

        #region 1. Common Fields + Easy-specific Input

        CopyCommonFields(currentState, result);
        result.HanabiScore = currentState.HanabiScore;

        #endregion

        #region 2. Reset Detection (TimeFrames == 0)

        if (currentState.TimeFrames == 0)
        {
            result.StatusCardTitle = "NORMAL\nPLAY";
            result.StatusCardSubtitle = "";
            result.ShowStaffRollTime = false;

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
        }

        // Copy section times
        result.SectionTimes = (int[])_sectionTimes.Clone();

        // Calculate current section time
        var prevSectionTotal = currentSection > 0 && _sectionCompletionTimes[currentSection - 1] > 0
            ? _sectionCompletionTimes[currentSection - 1]
            : 0;
        result.CurrentSectionTime = currentState.TimeFrames - prevSectionTotal;

        _previousLevel = currentState.Level;
        _previousSection = currentSection;

        // Speed Level = Level for Easy mode
        result.SpeedLevel = currentState.Level;
        result.MaxLockFrame = LockFrameConstants.EasyMaxLockFrame;

        #endregion

        #region 4. Section Time Display Formatting

        ComputeSectionTimeDisplays(result.SectionTimes, 2,
            out var timeDisplays, out var cumulativeDisplays);

        // Real-time display for current section
        if (currentSection >= 0 && currentSection < 2
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

        #region 5. Status Card

        result.RemainingStaffRollFrames = MaxStaffRollFrames - currentState.StaffRollTime;
        result.IsStaffRollActive = (currentState.GameFlag & 0x01) == 0x01;

        // Latch staff roll state once detected — flag clears before TimeFrames resets
        if (result.IsStaffRollActive)
            _staffRollLatch = true;

        if (_staffRollLatch)
        {
            result.StatusCardTitle = "STAFF ROLL!!";
            result.StatusCardSubtitle = result.RemainingStaffRollFrames > 0
                ? SectionTimeCalculator.FormatTime(result.RemainingStaffRollFrames)
                : "CLEAR!!";
            result.ShowStaffRollTime = true;
        }
        else
        {
            result.ShowStaffRollTime = false;
            result.StatusCardSubtitle = "";
            result.StatusCardTitle = "NORMAL\nPLAY";
        }

        #endregion

        return result;
    }

    public override void Reset()
    {
        _sectionCompletionTimes = new int[2];
        _sectionTimes = new int[2];
        _previousLevel = 0;
        _previousSection = 0;
        _staffRollLatch = false;
    }
}
