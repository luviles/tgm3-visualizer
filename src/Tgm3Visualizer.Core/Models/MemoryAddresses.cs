namespace Tgm3Visualizer.Core.Models;

/// <summary>
/// Memory addresses for TGM3
/// </summary>
public static class MemoryAddresses
{
    #region Game State / Common Variables

    /// <summary>
    /// Game type enum (4 bytes, Int32)
    /// </summary>
    public const int GameType = 0x4B3D5C;

    /// <summary>
    /// Game Process Status (1 byte)
    /// Address: 0x4BA9FC
    /// Value 3 = Playing
    /// </summary>
    public const int GameProcessStatus = 0x4BA9FC;

    /// <summary>
    /// Current level (2 bytes, UInt16)
    /// </summary>
    public const int Level = 0x4AE300;

    /// <summary>
    /// Game time in frames (4 bytes, Int32)
    /// </summary>
    public const int Time = 0x4AE33C;

    /// <summary>
    /// Field status flags (1 byte)
    /// Address: 0x4AE31C
    /// bit 6: 1 = Invisible Roll, 0 = Fading Roll
    /// </summary>
    public const int FieldStatus = 0x4AE31C;

    /// <summary>
    /// Game flags (1 byte)
    /// Address: 0x4AE31D
    /// bit 0: 1 = Staff Roll active, 0 = Normal play
    /// </summary>
    public const int GameFlag = 0x4AE31D;

    /// <summary>
    /// Final Grade (2 bytes, UInt16)
    /// Address: 0x4ACD8E
    /// </summary>
    public const int FinalGrade = 0x4ACD8E;

    #endregion

    #region Easy Mode

    /// <summary>
    /// Easy mode Hanabi score (2 bytes, UInt16)
    /// Address: 0x4AE358
    /// </summary>
    public const int HanabiScore = 0x4AE358;

    #endregion

    #region Master Mode

    /// <summary>
    /// Master mode exam flags (1 byte)
    /// Address: 0x4AE32B
    /// bit 1: Promotion Exam (1 = active)
    /// bit 4: Demotion Exam (1 = active)
    /// </summary>
    public const int MasterExamFlags = 0x4AE32B;

    /// <summary>
    /// Internal Grade (1 byte)
    /// Address: 0x4ACD97
    /// Increases when Awarded Grade Points reaches 100
    /// </summary>
    public const int InternalGrade = 0x4ACD97;

    /// <summary>
    /// Awarded grade points (1 byte)
    /// Address: 0x4ACD96
    /// </summary>
    public const int AwardedGradePoints = 0x4ACD96;

    /// <summary>
    /// Decay time counter (1 byte)
    /// Address: 0x4ACD9A
    /// When this reaches 0, Awarded Grade Points decreases by 1
    /// </summary>
    public const int DecayTime = 0x4ACD9A;

    /// <summary>
    /// Section grade points from COOLs (1 byte)
    /// Address: 0x4AE310
    /// </summary>
    public const int SectionGradePoints = 0x4AE310;

    /// <summary>
    /// Section Regret Count (1 byte)
    /// Address: 0x4AE316
    /// Does not directly affect Final Grade; instead, it immediately deducts from Internal Grade
    /// </summary>
    public const int SectionRegretCount = 0x4AE316;

    /// <summary>
    /// 70% checkpoint counter (1 byte)
    /// Address: 0x4ACD8C
    /// Starts at 0, increments by 1 each time a section's 70% checkpoint is passed
    /// </summary>
    public const int SeventyPercentCheckpointCount = 0x4ACD8C;

    /// <summary>
    /// Section COOL flag (1 byte)
    /// Address: 0x4AE306
    /// 1 when the current section's COOL condition is met, resets to 0 on next section
    /// </summary>
    public const int SectionCoolFlag = 0x4AE306;

    #endregion

    #region Sakura Mode

    /// <summary>
    /// Sakura mode stage limit time (4 bytes, Int32)
    /// Address: 0x4AE340
    /// Current stage time limit in frames
    /// </summary>
    public const int SakuraStageLimitTime = 0x4AE340;

    /// <summary>
    /// Sakura mode stage elapsed time (4 bytes, Int32)
    /// Address: 0x4AE348
    /// Hidden data - current stage elapsed time in frames
    /// </summary>
    public const int SakuraStageElapsedTime = 0x4AE348;

    /// <summary>
    /// Sakura mode current stage level (1 byte)
    /// Address: 0x4AE331
    /// Convert using SakuraGradeConverter for display
    /// </summary>
    public const int SakuraStageLevel = 0x4AE331;

    /// <summary>
    /// Sakura mode cleared stage level (1 byte)
    /// Address: 0x4AE332
    /// Number of stages cleared so far
    /// </summary>
    public const int SakuraClearedStageLevel = 0x4AE332;

    #endregion

    #region Shirase Mode

    /// <summary>
    /// Garbage Quota for Shirase Rising Garbage mode (1 byte)
    /// Address: 0x4AF2EF
    /// Increments +1 on piece placement, decrements -1 on line clear.
    /// When value reaches section threshold, garbage line is generated and value resets to 0.
    /// </summary>
    public const int GarbageQuota = 0x4AF2EF;

    #endregion

    #region Game Speed / Restraint State

    /// <summary>
    /// Animation delay frames (2 bytes)
    /// Address: 0x4AE244
    /// Frames remaining before next animation/action transition after piece lock
    /// Also used on menu timer, etc.
    /// </summary>
    public const int AnimationDelay = 0x4AE244;

    /// <summary>
    /// Lock delay frames (1 byte)
    /// Address: 0x4AF301
    /// Frames remaining before piece locks after touching the ground
    /// </summary>
    public const int LockDelay = 0x4AF301;

    /// <summary>
    /// Shift move reset count (1 byte)
    /// Address: 0x4AF2EA
    /// Number of move resets caused by block shifts during lock delay (WORLD rule). 10 = instant lock.
    /// </summary>
    public const int ShiftMoveResetCount = 0x4AF2EA;

    /// <summary>
    /// Rotation move reset count (1 byte)
    /// Address: 0x4AF2E8
    /// Number of move resets caused by piece rotation during lock delay (WORLD rule). 8 = instant lock.
    /// </summary>
    public const int RotationMoveResetCount = 0x4AF2E8;

    #endregion

    #region Staff Roll

    /// <summary>
    /// Staff roll progress time in frames (2 bytes, UInt16)
    /// Address: 0x4AB7F4
    /// Max value: 3625 frames
    /// </summary>
    public const int StaffRollTime = 0x4AB7F4;

    /// <summary>
    /// Staff Roll Tetris Count (2 bytes, UInt16)
    /// Address: 0x4ACDA8
    /// Counts tetris performed during staff roll sequence
    /// </summary>
    public const int StaffRollTetrisCount = 0x4ACDA8;

    /// <summary>
    /// Staff Roll Line Clear Count (2 bytes, UInt16)
    /// Address: 0x4ACDAA
    /// Counts lines cleared during staff roll sequence
    /// </summary>
    public const int StaffRollLineClear = 0x4ACDAA;

    #endregion

    #region Player Info

    /// <summary>
    /// Nickname first character ASCII (1 byte)
    /// </summary>
    public const int Nickname1 = 0x4AE250;

    /// <summary>
    /// Nickname second character ASCII (1 byte)
    /// </summary>
    public const int Nickname2 = 0x4AE251;

    /// <summary>
    /// Nickname third character ASCII (1 byte)
    /// </summary>
    public const int Nickname3 = 0x4AE252;

    /// <summary>
    /// Total playtime (4 bytes, max 99999)
    /// </summary>
    public const int Playtime = 0x4AE258;

    /// <summary>
    /// Easy mode high score - WORLD control mode (2 bytes)
    /// </summary>
    public const int WorldEasyHiScore = 0x4AE27C;

    /// <summary>
    /// Easy mode high score - CLASSIC control mode (2 bytes)
    /// </summary>
    public const int ClassicEasyHiScore = 0x4AE26C;

    /// <summary>
    /// Sakura mode high grade - WORLD control mode (1 byte)
    /// </summary>
    public const int WorldSakuraHiGrade = 0x4AE27E;

    /// <summary>
    /// Sakura mode high grade - CLASSIC control mode (1 byte)
    /// </summary>
    public const int ClassicSakuraHiGrade = 0x4AE26E;

    /// <summary>
    /// Master mode current recognized grade - WORLD control mode (1 byte)
    /// </summary>
    public const int WorldMasterCurrentGrade = 0x4AE270;

    /// <summary>
    /// Master mode current recognized grade - CLASSIC control mode (1 byte)
    /// </summary>
    public const int ClassicMasterCurrentGrade = 0x4AE260;

    /// <summary>
    /// Shirase mode high grade - WORLD control mode (1 byte)
    /// </summary>
    public const int WorldShiraseHiGrade = 0x4AE27F;

    /// <summary>
    /// Shirase mode high grade - CLASSIC control mode (1 byte)
    /// </summary>
    public const int ClassicShiraseHiGrade = 0x4AE26F;

    /// <summary>
    /// Master grade history start address - WORLD control mode (7 bytes, 0x4AE271-0x4AE277)
    /// Most recent at 0x4AE277, oldest at 0x4AE271
    /// </summary>
    public const int WorldMasterGradeHistoryStart = 0x4AE271;

    /// <summary>
    /// Master grade history start address - CLASSIC control mode (7 bytes)
    /// </summary>
    public const int ClassicMasterGradeHistoryStart = 0x4AE261;

    /// <summary>
    /// Demotion points - WORLD control mode (1 byte)
    /// Reaches 30 = Demotion Exam
    /// </summary>
    public const int WorldDemotionPoints = 0x4AE27A;

    /// <summary>
    /// Demotion points - CLASSIC control mode (1 byte)
    /// Reaches 30 = Demotion Exam
    /// </summary>
    public const int ClassicDemotionPoints = 0x4AE26A;

    /// <summary>
    /// Decoration Points (4 bytes, Int32)
    /// Login user's experience points. Max 100,000.
    /// </summary>
    public const int DecorationPoints = 0x4AE25C;

    #endregion

    #region Playfield

    /// <summary>
    /// 1P Playfield base address
    /// Address: 0x04AB9A8
    /// Playfield is 10 cells wide × 22 rows high
    /// Coordinate (0,0) is bottom-left corner
    /// Each cell is 8 bytes apart (type 1byte + brightness 1byte + 6bytes padding)
    /// Each row is 12 cells × 8 bytes = 96 bytes (10 visible + 2 boundary)
    ///
    /// Cell address formula: Base + (y × 96) + (x × 8)
    /// </summary>
    public const nint Playfield1P = 0x04AB9A8;

    #endregion
}
