using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Tgm3Visualizer.Core.Calculations;
using Tgm3Visualizer.Core.Models;
using Tgm3Visualizer.Services;

namespace Tgm3Visualizer.ViewModels;

public partial class MasterModeViewModel : ModeViewModelBase, ICommonPlayerInfoViewModel, ICommonLevelTimeViewModel, ICommonPlayfieldViewModel, ICommonStatusCardViewModel
{
    [ObservableProperty] private int _level;
    [ObservableProperty] private string _grade = "--";
    [ObservableProperty] private int _finalGrade;
    [ObservableProperty] private int _sectionGradePoints;
    [ObservableProperty] private int _sectionRegretCount;
    [ObservableProperty] private int _awardedGradePoints;
    [ObservableProperty] private string _decayTime = "0 Frame";
    [ObservableProperty] private int _internalGrade;
    [ObservableProperty] private string _internalGradeDisplay = "9";
    [ObservableProperty] private string _internalGradeText = "0 (Grade 9)";
    [ObservableProperty] private double _staffRollGradePoints;
    [ObservableProperty] private string _staffRollGradePointsDisplay = "+0";
    [ObservableProperty] private double _awardedGradePointsProgress;
    [ObservableProperty] private double _decayTimeProgress;
    [ObservableProperty] private string _formattedTime = "00:00:00";
    [ObservableProperty] private string _remainingStaffRollTime = "";
    [ObservableProperty] private string _gameMode = "";
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

    // Exam Status
    [ObservableProperty] private bool _isPromotionExam;
    [ObservableProperty] private bool _isDemotionExam;

    // Master Mode Specific (Grade History, Average, Demotion)
    public ObservableCollection<GradeHistoryItem> GradeHistoryItems { get; } = new();
    [ObservableProperty] private string _averageGrade = "N/A";
    [ObservableProperty] private string _demotionProgress = "0/30";

    // Invisible Roll Qualification
    [ObservableProperty] private bool _isSectionCoolQualified;
    [ObservableProperty] private bool _isInternalGradeQualified;
    [ObservableProperty] private bool _isMaxInternalGrade;

    // COOL display (max during session)
    [ObservableProperty] private int _coolDisplay;

    // Status Card
    [ObservableProperty] private string _statusCardTitle = "NORMAL\nPLAY";
    [ObservableProperty] private string _statusCardSubtitle = "";
    [ObservableProperty] private bool _isStaffRollActive;
    [ObservableProperty] private bool _showStaffRollTime;

    // Deadline thresholds
    [ObservableProperty] private string _coolDeadline = "N/A";
    [ObservableProperty] private string _regretDeadline = "N/A";

    // Grade up sound
    private int _previousCombinedGrade = -1;
    private readonly Windows.Media.Playback.MediaPlayer _gradeUpPlayer = new();

    // Playfield (10x22 grid, y=0 is bottom row)
    [ObservableProperty] private byte[,] _playfield = new byte[22, 10];

    // Playfield info
    [ObservableProperty] private int _lockDelay;
    [ObservableProperty] private int _maxLockFrame = LockFrameConstants.MasterMaxLockFrames[0];
    [ObservableProperty] private string _moveResetText = "Move Reset: Shift 0/10 | Rotate 0/8";

    public ObservableCollection<SectionInfo> Sections { get; } = new();

    public MasterModeViewModel(GameDataService gameDataService) : base(gameDataService)
    {
        GradeHistoryItems.Add(new GradeHistoryItem { GradeName = "No History", IsHighlighted = false });

        for (int i = 0; i < 10; i++)
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

        Grade = state.GradeDisplay;
        FinalGrade = state.FinalGrade;
        SectionGradePoints = state.SectionGradePoints;
        CoolDisplay = state.MaxCoolThisSession;
        SectionRegretCount = state.SectionRegretCount;
        AwardedGradePoints = state.AwardedGradePoints;
        InternalGrade = state.InternalGrade;
        InternalGradeDisplay = state.InternalGradeDisplay;
        InternalGradeText = state.InternalGradeText;
        StaffRollGradePoints = state.StaffRollGradePoints;
        StaffRollGradePointsDisplay = state.StaffRollGradePointsDisplay;

        #endregion

        #region 3. Staff Roll

        RemainingStaffRollTime = state.RemainingStaffRollTimeDisplay;

        #endregion

        #region 4. Progress

        AwardedGradePointsProgress = state.AwardedGradePointsProgress;
        DecayTime = state.DecayTimeDisplay;
        DecayTimeProgress = state.DecayTimeProgress;
        GameMode = state.GameModeDisplay;

        #endregion

        #region 5. Deadline

        CoolDeadline = state.CoolDeadlineDisplay;
        RegretDeadline = state.RegretDeadlineDisplay;

        #endregion

        #region 6. Qualification / Exam Status

        IsSectionCoolQualified = state.IsSectionCoolQualified;
        IsInternalGradeQualified = state.IsInternalGradeQualified;
        IsMaxInternalGrade = state.IsMaxInternalGrade;
        IsPromotionExam = state.IsPromotionExam;
        IsDemotionExam = state.IsDemotionExam;

        #endregion

        #region 7. Status Card

        IsStaffRollActive = state.IsStaffRollActive;
        this.ApplyCommonStatusCard(state);

        #endregion

        #region 8. Grade History + Demotion

        GradeHistoryItems.Clear();
        foreach (var entry in state.GradeHistoryEntries)
        {
            GradeHistoryItems.Add(new GradeHistoryItem
            {
                GradeName = entry.GradeName,
                IsHighlighted = entry.IsHighlighted
            });
        }
        if (GradeHistoryItems.Count == 0)
        {
            GradeHistoryItems.Add(new GradeHistoryItem { GradeName = "No History", IsHighlighted = false });
        }
        AverageGrade = string.IsNullOrEmpty(state.AverageGradeDisplay) ? "N/A" : state.AverageGradeDisplay;
        DemotionProgress = state.DemotionProgressDisplay;

        #endregion

        #region 9. Playfield

        this.ApplyCommonPlayfield(state);

        #endregion

        #region 10. Grade Up Sound

        if (_previousCombinedGrade >= 0 && state.CombinedGrade > _previousCombinedGrade && state.CombinedGrade <= 32)
        {
            PlayGradeUpSound();
        }
        _previousCombinedGrade = state.CombinedGrade;

        #endregion

        #region 11. Reset Handling

        if (state.TimeFrames == 0)
        {
            RemainingStaffRollTime = "";
            IsStaffRollActive = false;
            ShowStaffRollTime = false;
            StatusCardSubtitle = "";
            CoolDeadline = "N/A";
            RegretDeadline = "N/A";
            InternalGradeDisplay = "9";
            InternalGradeText = "0 (Grade 9)";
            _previousCombinedGrade = -1;
            foreach (var section in Sections)
            {
                section.Time = "";
                section.SeventyPercentTime = "";
                section.Status = SectionStatus.None;
                section.IsCool = false;
            }
            LockDelay = 0;
            MaxLockFrame = LockFrameConstants.MasterMaxLockFrames[0];
            MoveResetText = "Move Reset: Shift 0/10 | Rotate 0/8";
            return;
        }

        #endregion

        #region 12. Section Times Update

        for (int i = 0; i < Math.Min(Sections.Count, state.SectionTimes.Length); i++)
        {
            Sections[i].SeventyPercentTime = state.SeventyPercentTimeDisplays[i];

            if (!string.IsNullOrEmpty(state.SectionTimeDisplays[i]))
            {
                Sections[i].Time = state.SectionTimeDisplays[i];

                if (i == 9)
                {
                    Sections[i].Status = state.Regrets[i] ? SectionStatus.Regret : SectionStatus.Normal;
                    Sections[i].IsCool = false;
                }
                else
                {
                    Sections[i].Status = state.Regrets[i] ? SectionStatus.Regret : (state.Cools[i] ? SectionStatus.Cool : SectionStatus.Normal);
                    Sections[i].IsCool = state.Cools[i];
                }
            }
        }

        // Real-time display for current section
        int currentSection = state.Section;
        if (currentSection >= 0 && currentSection < Sections.Count
            && currentSection < state.SectionTimes.Length
            && state.SectionTimes[currentSection] == 0)
        {
            // Section Time: always counting
            Sections[currentSection].Time = state.SectionTimeDisplays[currentSection];

            // 70% Time: counting until x70, then frozen
            if (state.SeventyPercentTimes[currentSection] == 0)
                Sections[currentSection].SeventyPercentTime = state.SeventyPercentTimeDisplays[currentSection];

            // IsCool: apply when 70% is reached and Cool is achieved
            if (currentSection < state.Cools.Length && state.Cools[currentSection])
                Sections[currentSection].IsCool = true;

            // Status: Normal while in progress
            Sections[currentSection].Status = SectionStatus.Normal;
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
            System.Diagnostics.Debug.WriteLine($"[MasterModeViewModel] Failed to play GradeUp sound: {ex.Message}");
        }
    }
}
