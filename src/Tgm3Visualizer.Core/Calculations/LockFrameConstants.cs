namespace Tgm3Visualizer.Core.Calculations;

/// <summary>
/// Max lock frame constants per game mode.
/// Lock delay starts at MaxLockFrame when a piece touches the ground and counts down to 0.
/// </summary>
public static class LockFrameConstants
{
    /// <summary>
    /// Master mode max lock frames by speed level section.
    /// Index 0 = speed level 0-99, index 1 = 100-199, etc.
    /// Beyond the array, the last value is used.
    /// </summary>
    public static readonly int[] MasterMaxLockFrames = [30, 30, 30, 30, 30, 30, 30, 30, 30, 17, 17, 15, 15];

    /// <summary>
    /// Shirase mode max lock frames by speed level section.
    /// </summary>
    public static readonly int[] ShiraseMaxLockFrames = [18, 18, 17, 15, 15, 13, 12, 12, 12, 12, 12, 10, 8, 15];

    /// <summary>
    /// Easy mode max lock frame (constant)
    /// </summary>
    public const int EasyMaxLockFrame = 30;

    /// <summary>
    /// Sakura mode max lock frame (constant)
    /// </summary>
    public const int SakuraMaxLockFrame = 30;

    /// <summary>
    /// Look up max lock frame from a speed-level-indexed array.
    /// If speedLevel exceeds the array, the last value is returned.
    /// </summary>
    public static int GetMaxLockFrame(int[] frames, int speedLevel)
    {
        int index = speedLevel / 100;
        return index < frames.Length ? frames[index] : frames[frames.Length - 1];
    }
}
