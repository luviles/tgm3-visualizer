namespace Tgm3Visualizer.ViewModels;

/// <summary>
/// Common interface for playfield display properties
/// </summary>
public interface ICommonPlayfieldViewModel
{
    byte[,] Playfield { get; set; }
    int LockDelay { get; set; }
    int MaxLockFrame { get; set; }
    string MoveResetText { get; set; }
}
