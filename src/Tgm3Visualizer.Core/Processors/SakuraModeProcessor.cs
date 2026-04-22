using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Models;

namespace Tgm3Visualizer.Core.Processors;

/// <summary>
/// Processes game state for Sakura mode.
/// Tracks stage progress, jewel blocks, and stage time limits.
/// </summary>
public class SakuraModeProcessor : GameStateProcessorBase
{
    private int[] _stageTimes = new int[27];
    private bool[] _stageCleared = new bool[27];
    private bool[] _stageForfeited = new bool[27];
    private int _previousStageLevel = -1;
    private int _previousClearedStageLevel;
    private bool _clearedReached19;
    private bool _clearedReached20;
    private int _latchedCumulativeFrames;

    public override ProcessedGameState Process(GameState currentState)
    {
        var result = new ProcessedGameState();

        #region 1. Common Fields + Sakura-specific Fields

        CopyCommonFields(currentState, result);

        result.SakuraStageLimitTime = currentState.SakuraStageLimitTime;
        result.SakuraStageElapsedTime = currentState.SakuraStageElapsedTime;
        result.SakuraStageLevel = currentState.SakuraStageLevel;
        result.SakuraClearedStageLevel = currentState.SakuraClearedStageLevel;
        result.SakuraStageLevelDisplay = SakuraGradeConverter.ToCurrentStageName(currentState.SakuraStageLevel);

        #endregion

        #region 2. Reset Detection

        if (currentState.TimeFrames == 0)
        {
            Reset();
            return result;
        }

        #endregion

        #region 3. Stage Tracking

        int currentIdx = currentState.SakuraStageLevel;

        // Real-time: record elapsed time for current stage
        if (currentIdx >= 0 && currentIdx < 27)
        {
            _stageTimes[currentIdx] = currentState.SakuraStageElapsedTime;
        }

        // Clear detection: when ClearedStageLevel increases, mark current stage as cleared (green)
        if (currentState.SakuraClearedStageLevel > _previousClearedStageLevel && currentIdx >= 0 && currentIdx < 27)
        {
            _stageCleared[currentIdx] = true;
        }

        // Stage transition: if previous stage was not cleared, mark as forfeited (red)
        if (currentIdx > _previousStageLevel && _previousStageLevel >= 0 && _previousStageLevel < 27)
        {
            if (!_stageCleared[_previousStageLevel])
            {
                _stageForfeited[_previousStageLevel] = true;
            }
        }

        // Clone arrays to result
        result.SakuraStageTimes = (int[])_stageTimes.Clone();
        result.SakuraStageCleared = (bool[])_stageCleared.Clone();
        result.SakuraStageForfeited = (bool[])_stageForfeited.Clone();

        _previousClearedStageLevel = currentState.SakuraClearedStageLevel;
        _previousStageLevel = currentIdx;

        #endregion

        #region 4. Jewel Block Count (0x12-0x19)

        int jewelCount = 0;
        for (int y = 0; y < 22; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                byte cell = currentState.Playfield[y, x];
                if (cell >= 0x12 && cell <= 0x19)
                {
                    jewelCount++;
                }
            }
        }
        result.RemainingJewelBlocks = jewelCount;

        result.SpeedLevel = currentState.Level;
        result.MaxLockFrame = LockFrameConstants.SakuraMaxLockFrame;

        #endregion

        #region 5. EX Stage Tier Determination (Latch)

        // Set latch only during regular stages (index 0-19 = display stage 1-20)
        if (currentIdx <= 19)
        {
            if (!_clearedReached19 && currentState.SakuraClearedStageLevel >= 19)
            {
                _clearedReached19 = true;
            }

            if (!_clearedReached20 && currentState.SakuraClearedStageLevel >= 20)
            {
                _clearedReached20 = true;
                _latchedCumulativeFrames = 0;
                for (int i = 0; i < 20; i++)
                    _latchedCumulativeFrames += _stageTimes[i];
            }
        }

        // Determine EX tier based on latch
        if (_clearedReached20)
        {
            result.ExStageTier = _latchedCumulativeFrames > 5 * 60 * 60 ? 5 : 7;
        }
        else if (_clearedReached19)
        {
            result.ExStageTier = 3;
        }

        // Cleared visual status (priority: rainbow > green > normal)
        if (_clearedReached20)
            result.ClearedStageStatus = 2; // Rainbow
        else if (_clearedReached19)
            result.ClearedStageStatus = 1; // Green

        #endregion

        #region 6. Sakura Display Formatting

        // Stage limit time display
        result.SakuraStageLimitTimeDisplay = result.SakuraStageLimitTime > 0
            ? SectionTimeCalculator.FormatTime(result.SakuraStageLimitTime)
            : "00:00:00";

        // Stage time displays
        ComputeSectionTimeDisplays(result.SakuraStageTimes, 27,
            out var timeDisplays, out var cumulativeDisplays);
        result.SectionTimeDisplays = timeDisplays;
        result.CumulativeTimeDisplays = cumulativeDisplays;

        #endregion

        return result;
    }

    public override void Reset()
    {
        _stageTimes = new int[27];
        _stageCleared = new bool[27];
        _stageForfeited = new bool[27];
        _previousStageLevel = -1;
        _previousClearedStageLevel = 0;
        _clearedReached19 = false;
        _clearedReached20 = false;
        _latchedCumulativeFrames = 0;
    }
}
