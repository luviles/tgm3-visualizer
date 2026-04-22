using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Models;
using Tgm3Visualizer.Services;

namespace Tgm3Visualizer.ViewModels;

public partial class ShiraseModeViewModel : ModeViewModelBase, ICommonPlayerInfoViewModel, ICommonLevelTimeViewModel, ICommonPlayfieldViewModel, ICommonStatusCardViewModel
{
    [ObservableProperty] private int _level;
    [ObservableProperty] private string _grade = "--";
    [ObservableProperty] private int _finalGrade;
    [ObservableProperty] private int _sectionRegretCount;
    [ObservableProperty] private string _formattedTime = "00:00:00";
    [ObservableProperty] private string _controlMode = "";

    // Deadline
    [ObservableProperty] private string _regretDeadline = "N/A";

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
    [ObservableProperty] private int _maxLockFrame = LockFrameConstants.ShiraseMaxLockFrames[0];
    [ObservableProperty] private string _moveResetText = "Move Reset: Shift 0/10 | Rotate 0/8";

    // Status Card
    [ObservableProperty] private string _statusCardTitle = "NORMAL\nPLAY";
    [ObservableProperty] private string _statusCardSubtitle = "";
    [ObservableProperty] private bool _showStaffRollTime;

    // Garbage Quota (Rising Garbage mode, levels 500-999)
    [ObservableProperty] private string _garbageQuotaText = "";
    [ObservableProperty] private double _garbageQuotaValue;
    [ObservableProperty] private double _garbageQuotaMax;
    [ObservableProperty] private bool _showGarbageQuota;

    // Level Time Limits (Shirase mode)
    [ObservableProperty] private string _level500TimeLimit = "03:03:00";
    [ObservableProperty] private string _level1000TimeLimit = "06:06:00";
    [ObservableProperty] private bool _isLevel500Exceeded;
    [ObservableProperty] private bool _isLevel1000Exceeded;

    public ObservableCollection<SectionInfo> Sections { get; } = new();

    // Grade up sound
    private int _previousCombinedGrade = -1;
    private readonly Windows.Media.Playback.MediaPlayer _gradeUpPlayer = new();

    public ShiraseModeViewModel(GameDataService gameDataService) : base(gameDataService)
    {
        for (int i = 0; i < 13; i++)
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

        #region 2. Grade Display

        int combinedGrade = state.CombinedGrade;
        Grade = ShiraseGradeConverter.ToGradeName(combinedGrade);
        FinalGrade = state.FinalGrade;
        SectionRegretCount = state.SectionRegretCount;

        // Deadline
        RegretDeadline = state.RegretDeadlineDisplay;

        #endregion

        #region 3. Playfield

        this.ApplyCommonPlayfield(state);

        #endregion

        #region 4. Grade Up Sound

        if (_previousCombinedGrade >= 0 && combinedGrade > _previousCombinedGrade)
        {
            PlayGradeUpSound();
        }
        _previousCombinedGrade = combinedGrade;

        #endregion

        #region 5. Status Card

        this.ApplyCommonStatusCard(state);

        // Garbage Quota
        ShowGarbageQuota = state.MaxGarbageQuota > 0;
        if (ShowGarbageQuota)
        {
            GarbageQuotaText = state.GarbageQuotaText;
            GarbageQuotaValue = state.GarbageQuota;
            GarbageQuotaMax = state.MaxGarbageQuota;
        }
        else
        {
            GarbageQuotaText = "";
            GarbageQuotaValue = 0;
            GarbageQuotaMax = 1;
        }

        // Level Time Limits
        Level500TimeLimit = state.Level500TimeLimitDisplay;
        Level1000TimeLimit = state.Level1000TimeLimitDisplay;
        IsLevel500Exceeded = state.Level500TimeLimitExceeded;
        IsLevel1000Exceeded = state.Level1000TimeLimitExceeded;

        #endregion

        #region 6. Reset Handling

        if (state.TimeFrames == 0)
        {
            _previousCombinedGrade = -1;
            RegretDeadline = "N/A";
            StatusCardTitle = "NORMAL\nPLAY";
            StatusCardSubtitle = "";
            ShowStaffRollTime = false;
            ShowGarbageQuota = false;
            GarbageQuotaText = "";
            GarbageQuotaValue = 0;
            GarbageQuotaMax = 1;
            Level500TimeLimit = state.Level500TimeLimitDisplay;
            Level1000TimeLimit = state.Level1000TimeLimitDisplay;
            IsLevel500Exceeded = false;
            IsLevel1000Exceeded = false;
            foreach (var section in Sections)
            {
                section.Time = "";
                section.CumulativeTime = "";
                section.Status = SectionStatus.None;
            }
            LockDelay = 0;
            MaxLockFrame = LockFrameConstants.ShiraseMaxLockFrames[0];
            MoveResetText = "Move Reset: Shift 0/10 | Rotate 0/8";
            return;
        }

        #endregion

        #region 7. Section Times Update

        for (int i = 0; i < Math.Min(Sections.Count, state.SectionTimeDisplays.Length); i++)
        {
            if (!string.IsNullOrEmpty(state.SectionTimeDisplays[i]))
            {
                Sections[i].Time = state.SectionTimeDisplays[i];
                Sections[i].CumulativeTime = state.CumulativeTimeDisplays[i];
                Sections[i].Status = state.Regrets[i] ? SectionStatus.Regret : SectionStatus.Normal;
            }
        }

        #endregion
    }

    private void PlayGradeUpSound()
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Assets", "GradeUp.wav");
            _gradeUpPlayer.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(path));
            _gradeUpPlayer.Play();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ShiraseModeViewModel] Failed to play GradeUp sound: {ex.Message}");
        }
    }
}
