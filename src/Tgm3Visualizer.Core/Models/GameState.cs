using Tgm3Visualizer.Core.Calculations;

namespace Tgm3Visualizer.Core.Models;

/// <summary>
/// Raw game state read from memory
/// </summary>
public class GameState
{
    /// <summary>
    /// Current final grade value from memory (0x4ACD8E)
    /// </summary>
    public int FinalGrade { get; set; }

    /// <summary>
    /// Section grade points from COOLs
    /// </summary>
    public int SectionGradePoints { get; set; }

    /// <summary>
    /// Number of section regrets (debug display only)
    /// </summary>
    public int SectionRegretCount { get; set; }

    /// <summary>
    /// Awarded grade points (accumulates toward next grade)
    /// </summary>
    public int AwardedGradePoints { get; set; }

    /// <summary>
    /// Decay timer - when reaches 0, grade points decrease by 1
    /// </summary>
    public int DecayTime { get; set; }

    /// <summary>
    /// Internal Grade (address 0x4ACD97)
    /// Increases when Awarded Grade Points reaches 100
    /// </summary>
    public int InternalGrade { get; set; }

    /// <summary>
    /// Current level (0-999)
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Elapsed time in frames (60 fps)
    /// </summary>
    public int TimeFrames { get; set; }

    /// <summary>
    /// Current game type
    /// </summary>
    public GameType GameType { get; set; }

    /// <summary>
    /// Time in seconds
    /// </summary>
    public double TimeSeconds => TimeFrames / 60.0;

    /// <summary>
    /// Current section (0-9)
    /// </summary>
    public int Section => Level / 100;

    /// <summary>
    /// 70% checkpoint counter (address 0x4ACD8C)
    /// Starts at 0, increments by 1 each time a section's 70% checkpoint is passed
    /// </summary>
    public int SeventyPercentCheckpointCount { get; set; }

    /// <summary>
    /// Section COOL flag (address 0x4AE306)
    /// 1 when current section COOL condition is met, 0 otherwise
    /// </summary>
    public int SectionCoolFlag { get; set; }

    /// <summary>
    /// Staff roll progress in frames (0-3625)
    /// </summary>
    public int StaffRollTime { get; set; }

    /// <summary>
    /// Staff Roll Tetris Count (address 0x4ACDA8)
    /// Counts tetris performed during staff roll sequence
    /// </summary>
    public int StaffRollTetrisCount { get; set; }

    /// <summary>
    /// Staff Roll Line Clear Count (address 0x4ACDAA)
    /// Counts lines cleared during staff roll sequence
    /// </summary>
    public int StaffRollLineClear { get; set; }

    /// <summary>
    /// Easy mode Hanabi score (2 bytes from 0x4AE358)
    /// </summary>
    public int HanabiScore { get; set; }

    #region Player Info

    /// <summary>
    /// Player nickname (3 characters max)
    /// </summary>
    public string Nickname { get; set; } = "";

    /// <summary>
    /// Total playtime (max 99999)
    /// </summary>
    public int Playtime { get; set; }

    /// <summary>
    /// Decoration Points (login user's experience, max 100,000)
    /// </summary>
    public int DecorationPoints { get; set; }

    /// <summary>
    /// Easy mode high score - WORLD control mode
    /// </summary>
    public int WorldEasyHiScore { get; set; }

    /// <summary>
    /// Easy mode high score - CLASSIC control mode
    /// </summary>
    public int ClassicEasyHiScore { get; set; }

    /// <summary>
    /// Sakura mode high grade - WORLD control mode
    /// </summary>
    public int WorldSakuraHiGrade { get; set; }

    /// <summary>
    /// Sakura mode high grade - CLASSIC control mode
    /// </summary>
    public int ClassicSakuraHiGrade { get; set; }

    /// <summary>
    /// Master mode current recognized grade - WORLD control mode
    /// </summary>
    public int WorldMasterCurrentGrade { get; set; }

    /// <summary>
    /// Master mode current recognized grade - CLASSIC control mode
    /// </summary>
    public int ClassicMasterCurrentGrade { get; set; }

    /// <summary>
    /// Shirase mode high grade - WORLD control mode
    /// </summary>
    public int WorldShiraseHiGrade { get; set; }

    /// <summary>
    /// Shirase mode high grade - CLASSIC control mode
    /// </summary>
    public int ClassicShiraseHiGrade { get; set; }

    /// <summary>
    /// Master grade history - WORLD control mode (7 entries, oldest first)
    /// </summary>
    public int[] WorldMasterGradeHistory { get; set; } = new int[7];

    /// <summary>
    /// Master grade history - CLASSIC control mode (7 entries, oldest first)
    /// </summary>
    public int[] ClassicMasterGradeHistory { get; set; } = new int[7];

    /// <summary>
    /// Demotion points - WORLD control mode (30 = Demotion Exam)
    /// </summary>
    public int WorldDemotionPoints { get; set; }

    /// <summary>
    /// Demotion points - CLASSIC control mode (30 = Demotion Exam)
    /// </summary>
    public int ClassicDemotionPoints { get; set; }

    /// <summary>
    /// Game Process Status (1 byte from 0x4BA9FC)
    /// </summary>
    public int GameProcessStatus { get; set; }

    /// <summary>
    /// Field status flags (1 byte from 0x4AE31C)
    /// bit 6: Invisible field status
    /// </summary>
    public int FieldStatus { get; set; }

    /// <summary>
    /// Game flags (1 byte from 0x4AE31D)
    /// bit 0: Staff Roll active
    /// </summary>
    public int GameFlag { get; set; }

    /// <summary>
    /// Playfield data (10×22 grid)
    /// Index [y, x] where y=0 is bottom row, x=0 is left column
    /// Block types: 0=empty, 2=Red, 3=Green, 4=Pink, 5=Blue, 6=Orange, 7=Yellow, 8=Cyan
    /// </summary>
    public byte[,] Playfield { get; set; } = new byte[22, 10];

    /// <summary>
    /// Animation delay frames (2 bytes from 0x4AE244)
    /// </summary>
    public int AnimationDelay { get; set; }

    /// <summary>
    /// Lock delay frames (1 byte from 0x4AF301)
    /// </summary>
    public int LockDelay { get; set; }

    /// <summary>
    /// Shift move reset count - resets caused by block movement (1 byte from 0x4AF2EA)
    /// </summary>
    public int ShiftMoveResetCount { get; set; }

    /// <summary>
    /// Rotation move reset count - resets caused by piece rotation (1 byte from 0x4AF2E8)
    /// </summary>
    public int RotationMoveResetCount { get; set; }

    /// <summary>
    /// Garbage Quota counter for Shirase Rising Garbage mode (1 byte from 0x4AF2EF)
    /// Increments +1 on piece placement, decrements -1 on line clear.
    /// When value reaches section threshold, garbage line is generated and value resets to 0.
    /// </summary>
    public int GarbageQuota { get; set; }

    #endregion

    #region Sakura Mode

    /// <summary>
    /// Sakura mode stage limit time in frames (4 bytes from 0x4AE340)
    /// </summary>
    public int SakuraStageLimitTime { get; set; }

    /// <summary>
    /// Sakura mode stage elapsed time in frames (4 bytes from 0x4AE348)
    /// </summary>
    public int SakuraStageElapsedTime { get; set; }

    /// <summary>
    /// Sakura mode current stage level (1 byte from 0x4AE331)
    /// Convert using SakuraGradeConverter for display
    /// </summary>
    public int SakuraStageLevel { get; set; }

    /// <summary>
    /// Sakura mode cleared stage level (1 byte from 0x4AE332)
    /// </summary>
    public int SakuraClearedStageLevel { get; set; }

    #endregion

    /// <summary>
    /// Master mode exam flags (1 byte from 0x4AE32B)
    /// bit 1: Promotion Exam, bit 4: Demotion Exam
    /// </summary>
    public int MasterExamFlags { get; set; }
}

/// <summary>
/// Processed game state with calculated values
/// </summary>
public class ProcessedGameState
{
    public int FinalGrade { get; set; }
    public int InternalGrade { get; set; }
    public string InternalGradeDisplay { get; set; } = "";
    public int SectionGradePoints { get; set; }
    public int SectionRegretCount { get; set; }
    public int AwardedGradePoints { get; set; }
    public int DecayTime { get; set; }
    public int Level { get; set; }
    public int TimeFrames { get; set; }
    public double TimeSeconds { get; set; }
    public GameType GameType { get; set; }
    public int Section { get; set; }

    // Section times in frames
    public int[] SectionTimes { get; set; } = new int[10];

    // Cool/Regret status per section
    public bool[] Cools { get; set; } = new bool[9];
    public bool[] Regrets { get; set; } = new bool[10];
    public int TotalCools { get; set; }

    // Current section info
    public int CurrentSectionTime { get; set; }

    // Deadline thresholds (cumulative frames)
    public int CoolDeadline { get; set; }
    public int RegretDeadline { get; set; }

    /// <summary>
    /// Remaining staff roll time in frames
    /// </summary>
    public int RemainingStaffRollFrames { get; set; }

    /// <summary>
    /// Time from section start to level 70 crossing (70% Time) per section in frames
    /// </summary>
    public int[] SeventyPercentTimes { get; set; } = new int[10];

    /// <summary>
    /// Staff Roll Grade Points (calculated based on Invisible/Fading roll)
    /// </summary>
    public double StaffRollGradePoints { get; set; }

    /// <summary>
    /// Staff Roll Clear Bonus Points (non-zero only when clear applied AND staff roll active)
    /// </summary>
    public double StaffRollClearPoints { get; set; }

    // Computed properties
    public int CombinedGrade { get; set; }
    public double AwardedGradePointsProgress { get; set; }
    public int DecayTimeRemaining { get; set; }
    public double DecayTimeProgress { get; set; }
    public bool IsSectionCoolQualified { get; set; }
    public bool IsInternalGradeQualified { get; set; }
    public bool IsMaxInternalGrade { get; set; }

    // Session-tracked values (set by MasterModeProcessor)
    public int MaxCoolThisSession { get; set; }
    public double MaxStaffRollGradePoints { get; set; }
    public bool IsStaffRollCleared { get; set; }

    // Status Card
    public string StatusCardTitle { get; set; } = "NORMAL\nPLAY";
    public string StatusCardSubtitle { get; set; } = "";
    public bool ShowStaffRollTime { get; set; }

    // Exam
    public bool IsPromotionExam { get; set; }
    public bool IsDemotionExam { get; set; }

    /// <summary>
    /// Master mode exam flags raw byte (from 0x4AE32B)
    /// bit 1: Promotion Exam, bit 4: Demotion Exam
    /// </summary>
    public int MasterExamFlags { get; set; }

    #region Player Info

    public string Nickname { get; set; } = "";
    public int Playtime { get; set; }
    public int DecorationPoints { get; set; }
    public bool IsLoggedIn { get; set; }
    public int WorldEasyHiScore { get; set; }
    public int ClassicEasyHiScore { get; set; }
    public int WorldSakuraHiGrade { get; set; }
    public int ClassicSakuraHiGrade { get; set; }
    public int WorldMasterCurrentGrade { get; set; }
    public int ClassicMasterCurrentGrade { get; set; }
    public int WorldShiraseHiGrade { get; set; }
    public int ClassicShiraseHiGrade { get; set; }
    public int[] WorldMasterGradeHistory { get; set; } = new int[7];
    public int[] ClassicMasterGradeHistory { get; set; } = new int[7];
    public int WorldDemotionPoints { get; set; }
    public int ClassicDemotionPoints { get; set; }

    /// <summary>
    /// Game Process Status (raw value from 0x4BA9FC)
    /// </summary>
    public int GameProcessStatus { get; set; }

    /// <summary>
    /// Whether the field is in invisible mode (bit 6 of 0x4AE31C)
    /// </summary>
    public bool IsInvisible { get; set; }

    /// <summary>
    /// Whether staff roll is active (bit 0 of 0x4AE31D)
    /// </summary>
    public bool IsStaffRollActive { get; set; }

    /// <summary>
    /// Control mode: "CLASSIC" or "WORLD" (bit 4 of 0x4AE31D)
    /// </summary>
    public string ControlMode { get; set; } = "";

    /// <summary>
    /// Playfield data (10×22 grid)
    /// Index [y, x] where y=0 is bottom row, x=0 is left column
    /// Block types: 0=empty, 2=Red, 3=Green, 4=Pink, 5=Blue, 6=Orange, 7=Yellow, 8=Cyan
    /// </summary>
    public byte[,] Playfield { get; set; } = new byte[22, 10];

    /// <summary>
    /// Animation delay frames remaining (2 bytes from 0x4AE244)
    /// </summary>
    public int AnimationDelay { get; set; }

    /// <summary>
    /// Lock delay frames remaining (from 0x4AF301)
    /// </summary>
    public int LockDelay { get; set; }

    /// <summary>
    /// Shift move reset count during lock delay - caused by block movement (WORLD rule only)
    /// </summary>
    public int ShiftMoveResetCount { get; set; }

    /// <summary>
    /// Rotation move reset count during lock delay - caused by piece rotation (WORLD rule only)
    /// </summary>
    public int RotationMoveResetCount { get; set; }

    /// <summary>
    /// Calculated speed level: Level + (CoolCount * 100) for Master, Level for others
    /// </summary>
    public int SpeedLevel { get; set; }

    /// <summary>
    /// Max lock frame for current mode and speed level
    /// </summary>
    public int MaxLockFrame { get; set; }

    /// <summary>
    /// Current garbage quota value (from memory, Shirase Rising Garbage mode)
    /// </summary>
    public int GarbageQuota { get; set; }

    /// <summary>
    /// Max garbage quota threshold for current level section (Shirase Rising Garbage mode)
    /// When GarbageQuota reaches this value, a garbage line is generated.
    /// </summary>
    public int MaxGarbageQuota { get; set; }

    /// <summary>
    /// Whether Level 500 was reached after its time limit (Shirase mode)
    /// </summary>
    public bool Level500TimeLimitExceeded { get; set; }

    /// <summary>
    /// Whether Level 1000 was reached after its time limit (Shirase mode)
    /// </summary>
    public bool Level1000TimeLimitExceeded { get; set; }

    /// <summary>
    /// Easy mode Hanabi score
    /// </summary>
    public int HanabiScore { get; set; }

    #endregion

    #region Sakura Mode

    /// <summary>
    /// Sakura mode stage limit time in frames
    /// </summary>
    public int SakuraStageLimitTime { get; set; }

    /// <summary>
    /// Sakura mode stage elapsed time in frames
    /// </summary>
    public int SakuraStageElapsedTime { get; set; }

    /// <summary>
    /// Sakura mode current stage level (raw byte)
    /// </summary>
    public int SakuraStageLevel { get; set; }

    /// <summary>
    /// Sakura mode current stage level display string (converted via SakuraGradeConverter)
    /// </summary>
    public string SakuraStageLevelDisplay { get; set; } = "1";

    /// <summary>
    /// Sakura mode cleared stage level
    /// </summary>
    public int SakuraClearedStageLevel { get; set; }

    /// <summary>
    /// Remaining jewel blocks on the playfield (blocks with value 0x12-0x19)
    /// </summary>
    public int RemainingJewelBlocks { get; set; }

    /// <summary>
    /// Stage times in frames for Sakura mode (27 stages: 1-20, EX1-EX7)
    /// Index 0 = stage 1, index 26 = EX7
    /// </summary>
    public int[] SakuraStageTimes { get; set; } = new int[27];

    /// <summary>
    /// Whether each Sakura stage was cleared normally (green)
    /// </summary>
    public bool[] SakuraStageCleared { get; set; } = new bool[27];

    /// <summary>
    /// Whether each Sakura stage was forfeited due to time limit (red)
    /// </summary>
    public bool[] SakuraStageForfeited { get; set; } = new bool[27];

    /// <summary>
    /// EX Stage tier: 0=not determined, 3=EX3, 5=EX5, 7=EX7
    /// </summary>
    public int ExStageTier { get; set; }

    /// <summary>
    /// Cleared stage visual status: 0=normal, 1=green (CLEARED=19), 2=rainbow (CLEARED=20)
    /// </summary>
    public int ClearedStageStatus { get; set; }

    #endregion

    #region Common Display

    /// <summary>
    /// Formatted move reset text showing shift/rotate counts (WORLD rule) or "WORLD RULE ONLY" (CLASSIC rule)
    /// </summary>
    public string MoveResetText { get; set; } = "Move Reset: Shift 0/10 | Rotate 0/8";

    /// <summary>
    /// Formatted elapsed time string (MM:SS:ss) or "00:00:00" if not started
    /// </summary>
    public string FormattedTime { get; set; } = "00:00:00";

    /// <summary>
    /// Formatted regret deadline display (e.g., "01:23:45" or "N/A")
    /// </summary>
    public string RegretDeadlineDisplay { get; set; } = "N/A";

    #endregion

    #region Section Display

    /// <summary>
    /// Formatted individual section time displays (empty string for unstarted sections, max 27)
    /// </summary>
    public string[] SectionTimeDisplays { get; set; } = new string[27];

    /// <summary>
    /// Formatted cumulative section time displays (empty string for unstarted sections, max 27)
    /// </summary>
    public string[] CumulativeTimeDisplays { get; set; } = new string[27];

    /// <summary>
    /// Formatted 70% time displays per section (Master mode only)
    /// </summary>
    public string[] SeventyPercentTimeDisplays { get; set; } = new string[10];

    #endregion

    #region Shirase Display

    /// <summary>
    /// Shirase: Formatted garbage quota display (e.g., "12/20")
    /// </summary>
    public string GarbageQuotaText { get; set; } = "";

    /// <summary>
    /// Shirase: Formatted Level 500 time limit display
    /// </summary>
    public string Level500TimeLimitDisplay { get; set; } = "";

    /// <summary>
    /// Shirase: Formatted Level 1000 time limit display
    /// </summary>
    public string Level1000TimeLimitDisplay { get; set; } = "";

    #endregion

    #region Master Display

    /// <summary>
    /// Master: Formatted grade name display (e.g., "S9", "GM")
    /// </summary>
    public string GradeDisplay { get; set; } = "--";

    /// <summary>
    /// Master: Formatted internal grade text (e.g., "18 (Grade S4)")
    /// </summary>
    public string InternalGradeText { get; set; } = "0 (Grade 9)";

    /// <summary>
    /// Master: Formatted staff roll grade points display (e.g., "+1.6")
    /// </summary>
    public string StaffRollGradePointsDisplay { get; set; } = "+0";

    /// <summary>
    /// Master: Game mode display string (e.g., "MASTER", "QUALIFIED MASTER")
    /// </summary>
    public string GameModeDisplay { get; set; } = "";

    /// <summary>
    /// Master: Formatted decay time display (e.g., "125 Frames")
    /// </summary>
    public string DecayTimeDisplay { get; set; } = "0 Frame";

    /// <summary>
    /// Master: Formatted demotion progress (e.g., "15/30")
    /// </summary>
    public string DemotionProgressDisplay { get; set; } = "0/30";

    /// <summary>
    /// Master: Formatted remaining staff roll time display (e.g., "00:45:23" or "CLEAR!!")
    /// </summary>
    public string RemainingStaffRollTimeDisplay { get; set; } = "";

    /// <summary>
    /// Master: Formatted cool deadline display (e.g., "01:23:45" or "N/A")
    /// </summary>
    public string CoolDeadlineDisplay { get; set; } = "N/A";

    /// <summary>
    /// Master: Pre-computed grade history entries for display
    /// </summary>
    public List<GradeHistoryEntry> GradeHistoryEntries { get; set; } = new();

    /// <summary>
    /// Master: Average grade display string from grade history
    /// </summary>
    public string AverageGradeDisplay { get; set; } = "";

    /// <summary>
    /// Master: Demotion points selected by ControlMode
    /// </summary>
    public int SelectedDemotionPoints { get; set; }

    #endregion

    #region Sakura Display

    /// <summary>
    /// Sakura: Formatted stage limit time display
    /// </summary>
    public string SakuraStageLimitTimeDisplay { get; set; } = "00:00:00";

    #endregion
}

