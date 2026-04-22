using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Models;
using Tgm3Visualizer.Services;

namespace Tgm3Visualizer.ViewModels;

public partial class EasyModeViewModel : ModeViewModelBase, ICommonPlayerInfoViewModel, ICommonLevelTimeViewModel, ICommonPlayfieldViewModel, ICommonStatusCardViewModel
{
    [ObservableProperty] private int _level;
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
    [ObservableProperty] private int _maxLockFrame = LockFrameConstants.EasyMaxLockFrame;
    [ObservableProperty] private string _moveResetText = "Move Reset: Shift 0/10 | Rotate 0/8";

    // Hanabi Score
    [ObservableProperty] private int _hanabiScoreValue;

    // Status Card
    [ObservableProperty] private string _statusCardTitle = "NORMAL\nPLAY";
    [ObservableProperty] private string _statusCardSubtitle = "";
    [ObservableProperty] private bool _showStaffRollTime;

    public ObservableCollection<SectionInfo> Sections { get; } = new();

    public EasyModeViewModel(GameDataService gameDataService) : base(gameDataService)
    {
        for (int i = 0; i < 2; i++)
        {
            Sections.Add(new SectionInfo
            {
                Range = SectionTimeCalculator.GetSectionRange(i),
                Time = "",
                Status = SectionStatus.None
            });
        }
    }

    public override void UpdateFromGameState(ProcessedGameState state)
    {
        #region 1. Common Properties

        this.ApplyCommonLevelTime(state);
        this.ApplyCommonPlayerInfo(state);
        ControlMode = state.ControlMode;

        #endregion

        #region 2. Hanabi Score

        HanabiScoreValue = state.HanabiScore;

        #endregion

        #region 3. Playfield

        this.ApplyCommonPlayfield(state);

        #endregion

        #region 4. Status Card

        this.ApplyCommonStatusCard(state);

        #endregion

        #region 5. Reset Handling

        if (state.TimeFrames == 0)
        {
            HanabiScoreValue = 0;
            StatusCardTitle = "NORMAL\nPLAY";
            StatusCardSubtitle = "";
            ShowStaffRollTime = false;
            foreach (var section in Sections)
            {
                section.Time = "";
                section.CumulativeTime = "";
                section.Status = SectionStatus.None;
            }
            return;
        }

        #endregion

        #region 6. Section Times Update

        for (int i = 0; i < Sections.Count; i++)
        {
            if (!string.IsNullOrEmpty(state.SectionTimeDisplays[i]))
            {
                Sections[i].Time = state.SectionTimeDisplays[i];
                Sections[i].CumulativeTime = state.CumulativeTimeDisplays[i];
                Sections[i].Status = SectionStatus.Normal;
            }
        }

        #endregion
    }
}
