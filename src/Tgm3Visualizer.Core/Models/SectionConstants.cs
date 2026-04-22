namespace Tgm3Visualizer.Core.Models;

/// <summary>
/// Time constants for Cool/Regret detection (in seconds at 60fps)
/// </summary>
public static class SectionConstants
{
    /// <summary>
    /// Maximum section time for Cool (seconds)
    /// Each section (0-8) has its own threshold
    /// Section 9 has no Cool
    /// </summary>
    public static readonly int[] CoolTimes = { 52, 52, 49, 45, 45, 42, 42, 38, 38 };

    /// <summary>
    /// Maximum section time before Regret (seconds)
    /// Exceeding these times triggers a Regret
    /// </summary>
    public static readonly int[] RegretTimes = { 90, 75, 75, 68, 60, 60, 50, 50, 50, 50 };

    /// <summary>
    /// Shirase Regret time limits (seconds)
    /// Sections 0-1: 60s, Sections 2-12: 50s
    /// </summary>
    public static readonly int[] ShiraseRegretTimes = { 60, 60, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };

    /// <summary>
    /// Convert seconds to frames (60fps)
    /// </summary>
    public static int SecondsToFrames(int seconds) => seconds * 60;

    /// <summary>
    /// Convert frames to seconds
    /// </summary>
    public static double FramesToSeconds(int frames) => frames / 60.0;

    /// <summary>
    /// Get Cool threshold in frames for a section
    /// </summary>
    public static int GetCoolThresholdFrames(int section)
        => section < CoolTimes.Length ? CoolTimes[section] * 60 : int.MaxValue;

    /// <summary>
    /// Get Regret threshold in frames for a section
    /// </summary>
    public static int GetRegretThresholdFrames(int section)
        => section < RegretTimes.Length ? RegretTimes[section] * 60 : int.MaxValue;

    /// <summary>
    /// Get Shirase Regret threshold in frames for a section
    /// </summary>
    public static int GetShiraseRegretThresholdFrames(int section)
        => section < ShiraseRegretTimes.Length ? ShiraseRegretTimes[section] * 60 : int.MaxValue;

    /// <summary>
    /// Shirase Level Time Limits (frames at 60fps)
    /// WORLD: Level 500 in 3 min 3 sec (183s), Level 1000 in 6 min 6 sec (366s)
    /// </summary>
    public const int ShiraseWorldLevel500Limit = 183 * 60;   // 10980 frames
    public const int ShiraseWorldLevel1000Limit = 366 * 60;  // 21960 frames

    /// <summary>
    /// Shirase Level Time Limits (frames at 60fps)
    /// CLASSIC: Level 500 in 2 min 28 sec (148s), Level 1000 in 4 min 56 sec (296s)
    /// </summary>
    public const int ShiraseClassicLevel500Limit = 148 * 60;  // 8880 frames
    public const int ShiraseClassicLevel1000Limit = 296 * 60;  // 17760 frames
}
