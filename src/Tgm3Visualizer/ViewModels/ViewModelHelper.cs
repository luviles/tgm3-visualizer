using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Models;

namespace Tgm3Visualizer.ViewModels;

/// <summary>
/// Extension methods for applying common property mappings from ProcessedGameState.
/// </summary>
public static class ViewModelHelper
{
    public static void ApplyCommonPlayerInfo(this ICommonPlayerInfoViewModel vm, ProcessedGameState state)
    {
        vm.Nickname = state.Nickname;
        vm.IsLoggedIn = state.IsLoggedIn;
        vm.DecorationPoints = state.DecorationPoints.ToString("N0");
        vm.WorldEasyHiScore = $"H{state.WorldEasyHiScore}";
        vm.ClassicEasyHiScore = $"H{state.ClassicEasyHiScore}";
        vm.WorldMasterGrade = GradeConverter.ToGradeName(state.WorldMasterCurrentGrade);
        vm.ClassicMasterGrade = GradeConverter.ToGradeName(state.ClassicMasterCurrentGrade);
        vm.WorldSakuraGrade = SakuraGradeConverter.ToGradeName(state.WorldSakuraHiGrade);
        vm.ClassicSakuraGrade = SakuraGradeConverter.ToGradeName(state.ClassicSakuraHiGrade);
        vm.WorldShiraseGrade = ShiraseGradeConverter.ToGradeName(state.WorldShiraseHiGrade);
        vm.ClassicShiraseGrade = ShiraseGradeConverter.ToGradeName(state.ClassicShiraseHiGrade);
    }

    public static void ApplyCommonLevelTime(this ICommonLevelTimeViewModel vm, ProcessedGameState state)
    {
        vm.Level = state.Level;
        vm.FormattedTime = state.FormattedTime;
    }

    /// <summary>
    /// Apply common playfield info: Playfield, LockDelay, MaxLockFrame, MoveResetText
    /// </summary>
    public static void ApplyCommonPlayfield(this ICommonPlayfieldViewModel vm, ProcessedGameState state)
    {
        vm.Playfield = state.Playfield;
        vm.LockDelay = state.LockDelay;
        vm.MaxLockFrame = state.MaxLockFrame;
        vm.MoveResetText = state.MoveResetText;
    }

    /// <summary>
    /// Apply common status card: StatusCardTitle, StatusCardSubtitle, ShowStaffRollTime
    /// </summary>
    public static void ApplyCommonStatusCard(this ICommonStatusCardViewModel vm, ProcessedGameState state)
    {
        vm.StatusCardTitle = state.StatusCardTitle;
        vm.StatusCardSubtitle = state.StatusCardSubtitle;
        vm.ShowStaffRollTime = state.ShowStaffRollTime;
    }
}
