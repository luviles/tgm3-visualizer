using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Tgm3Visualizer.Brushes;
using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Models;
using Tgm3Visualizer.Services;

namespace Tgm3Visualizer.ViewModels;

public partial class SakuraModeViewModel : ModeViewModelBase, ICommonPlayerInfoViewModel, ICommonPlayfieldViewModel
{
    [ObservableProperty] private string _formattedTime = "00:00:00";
    [ObservableProperty] private string _controlMode = "";

    // Player Info
    [ObservableProperty] private string _nickname = "";
    [ObservableProperty] private string _decorationPoints = "";
    [ObservableProperty] private bool _isLoggedIn;
    [ObservableProperty] private string _worldEasyHiScore = "";
    [ObservableProperty] private string _classicEasyHiScore = "";
    [ObservableProperty] private string _worldMasterGrade = "";
    [ObservableProperty] private string _classicMasterGrade = "";
    [ObservableProperty] private string _worldSakuraGrade = "";
    [ObservableProperty] private string _classicSakuraGrade = "";
    [ObservableProperty] private string _worldShiraseGrade = "";
    [ObservableProperty] private string _classicShiraseGrade = "";

    // Playfield (10x22 grid, y=0 is bottom row)
    [ObservableProperty] private byte[,] _playfield = new byte[22, 10];

    // Playfield info
    [ObservableProperty] private int _lockDelay;
    [ObservableProperty] private int _maxLockFrame = LockFrameConstants.SakuraMaxLockFrame;
    [ObservableProperty] private string _moveResetText = "Move Reset: Shift 0/10 | Rotate 0/8";

    // Sakura Mode specific
    [ObservableProperty] private string _sakuraStageLevelDisplay = "1";
    [ObservableProperty] private int _sakuraClearedStageLevel;
    [ObservableProperty] private string _sakuraStageLimitTime = "";
    [ObservableProperty] private int _remainingJewelBlocks;

    [ObservableProperty] private int _exStageTier;
    [ObservableProperty] private Brush _clearedForeground = new SolidColorBrush(Colors.White);

    private static readonly SolidColorBrush ClearedGreenBrush = new(Colors.LimeGreen);

    public ObservableCollection<SectionInfo> Sections { get; } = new();

    public SakuraModeViewModel(GameDataService gameDataService) : base(gameDataService)
    {
        for (int i = 0; i < 27; i++)
        {
            Sections.Add(new SectionInfo
            {
                Range = SakuraGradeConverter.ToGradeName(i + 1),
                Time = "",
                Status = SectionStatus.Normal
            });
        }
    }

    public override void UpdateFromGameState(ProcessedGameState state)
    {
        #region 1. Common Properties

        this.ApplyCommonPlayerInfo(state);
        ControlMode = state.ControlMode;

        #endregion

        #region 2. Time Display

        FormattedTime = state.FormattedTime;

        #endregion

        #region 3. Playfield

        this.ApplyCommonPlayfield(state);

        #endregion

        #region 4. Sakura-specific

        SakuraStageLevelDisplay = state.SakuraStageLevelDisplay;
        SakuraClearedStageLevel = state.SakuraClearedStageLevel;
        SakuraStageLimitTime = state.SakuraStageLimitTimeDisplay;
        RemainingJewelBlocks = state.RemainingJewelBlocks;
        ExStageTier = state.ExStageTier;
        ClearedForeground = state.ClearedStageStatus switch
        {
            2 => RainbowBrushes.Rainbow,
            1 => ClearedGreenBrush,
            _ => new SolidColorBrush(Colors.White)
        };

        #endregion

        #region 5. Reset Handling

        if (state.TimeFrames == 0)
        {
            foreach (var section in Sections)
            {
                section.Time = "";
                section.CumulativeTime = "";
                section.Status = SectionStatus.Normal;
            }
            return;
        }

        #endregion

        #region 6. Stage Times Update

        for (int i = 0; i < Math.Min(Sections.Count, state.SakuraStageTimes.Length); i++)
        {
            if (!string.IsNullOrEmpty(state.SectionTimeDisplays[i]))
            {
                Sections[i].Time = state.SectionTimeDisplays[i];
                Sections[i].CumulativeTime = state.CumulativeTimeDisplays[i];
            }

            // Status: cleared (green) > forfeited (red) > in progress (white) > not started (dim)
            if (state.SakuraStageCleared[i])
                Sections[i].Status = SectionStatus.Cool;
            else if (state.SakuraStageForfeited[i])
                Sections[i].Status = SectionStatus.Regret;
            else if (state.SakuraStageTimes[i] > 0)
                Sections[i].Status = SectionStatus.Normal;
            else
                Sections[i].Status = SectionStatus.None;
        }

        #endregion
    }
}
