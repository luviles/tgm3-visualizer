using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Interfaces;
using Tgm3Visualizer.Core.Models;

namespace Tgm3Visualizer.Core.Processors;

/// <summary>
/// Base class for game state processors providing common field copying and display formatting.
/// Mode-specific logic (section tracking, grade calculations) remains in subclasses.
/// </summary>
public abstract class GameStateProcessorBase : IGameStateProcessor
{
    public abstract ProcessedGameState Process(GameState currentState);
    public abstract void Reset();

    /// <summary>
    /// Copies common fields from GameState to ProcessedGameState.
    /// This includes Level, Time, PlayerInfo, Playfield, grade-related fields,
    /// and common display formatting (MoveResetText, FormattedTime).
    /// </summary>
    public static void CopyCommonFields(GameState currentState, ProcessedGameState result)
    {
        #region Level & Time

        result.Level = currentState.Level;
        result.TimeFrames = currentState.TimeFrames;
        result.TimeSeconds = currentState.TimeSeconds;
        result.GameType = currentState.GameType;
        result.Section = currentState.Section;

        #endregion

        #region Grade-related

        result.FinalGrade = currentState.FinalGrade;
        result.SectionGradePoints = currentState.SectionGradePoints;
        result.SectionRegretCount = currentState.SectionRegretCount;
        result.AwardedGradePoints = currentState.AwardedGradePoints;
        result.DecayTime = currentState.DecayTime;
        result.InternalGrade = currentState.InternalGrade;

        #endregion

        #region Player Info

        result.Nickname = currentState.Nickname;
        result.Playtime = currentState.Playtime;
        result.DecorationPoints = currentState.DecorationPoints;
        // Login status: if all 3 nickname characters are whitespace, user is not logged in
        result.IsLoggedIn = !string.IsNullOrEmpty(currentState.Nickname) && currentState.Nickname.Trim().Length > 0;
        result.WorldEasyHiScore = currentState.WorldEasyHiScore;
        result.ClassicEasyHiScore = currentState.ClassicEasyHiScore;
        result.WorldSakuraHiGrade = currentState.WorldSakuraHiGrade;
        result.ClassicSakuraHiGrade = currentState.ClassicSakuraHiGrade;
        result.WorldMasterCurrentGrade = currentState.WorldMasterCurrentGrade;
        result.ClassicMasterCurrentGrade = currentState.ClassicMasterCurrentGrade;
        result.WorldShiraseHiGrade = currentState.WorldShiraseHiGrade;
        result.ClassicShiraseHiGrade = currentState.ClassicShiraseHiGrade;
        result.WorldMasterGradeHistory = currentState.WorldMasterGradeHistory;
        result.ClassicMasterGradeHistory = currentState.ClassicMasterGradeHistory;
        result.WorldDemotionPoints = currentState.WorldDemotionPoints;
        result.ClassicDemotionPoints = currentState.ClassicDemotionPoints;

        #endregion

        #region Status

        result.GameProcessStatus = currentState.GameProcessStatus;
        result.IsInvisible = (currentState.FieldStatus & 0x40) == 0x40;
        result.IsStaffRollActive = (currentState.GameFlag & 0x01) == 0x01;
        result.ControlMode = (currentState.GameFlag & 0x10) == 0x10 ? "WORLD" : "CLASSIC";

        #endregion

        #region Playfield

        result.Playfield = (byte[,])currentState.Playfield.Clone();

        #endregion

        #region Animation / Lock Delay

        result.AnimationDelay = currentState.AnimationDelay;
        result.LockDelay = currentState.LockDelay;
        result.ShiftMoveResetCount = currentState.ShiftMoveResetCount;
        result.RotationMoveResetCount = currentState.RotationMoveResetCount;

        #endregion

        #region Garbage Quota

        result.GarbageQuota = currentState.GarbageQuota;

        #endregion

        #region Master Exam Flags

        result.MasterExamFlags = currentState.MasterExamFlags;

        #endregion

        #region Common Display Formatting

        result.MoveResetText = result.ControlMode == "WORLD"
            ? $"Move Reset: Shift {currentState.ShiftMoveResetCount}/10 | Rotate {currentState.RotationMoveResetCount}/8"
            : "Move Reset: WORLD RULE ONLY";

        result.FormattedTime = currentState.TimeFrames > 0
            ? SectionTimeCalculator.FormatTime(currentState.TimeFrames)
            : "00:00:00";

        #endregion
    }

    /// <summary>
    /// Computes formatted section time displays from individual section times.
    /// Returns SectionTimeDisplays and CumulativeTimeDisplays arrays.
    /// </summary>
    protected static void ComputeSectionTimeDisplays(
        int[] sectionTimes, int count,
        out string[] sectionTimeDisplays, out string[] cumulativeTimeDisplays)
    {
        sectionTimeDisplays = new string[sectionTimes.Length];
        cumulativeTimeDisplays = new string[sectionTimes.Length];

        int cumulative = 0;
        for (int i = 0; i < count && i < sectionTimes.Length; i++)
        {
            if (sectionTimes[i] > 0)
            {
                cumulative += sectionTimes[i];
                sectionTimeDisplays[i] = SectionTimeCalculator.FormatTime(sectionTimes[i]);
                cumulativeTimeDisplays[i] = SectionTimeCalculator.FormatTime(cumulative);
            }
        }
    }
}
