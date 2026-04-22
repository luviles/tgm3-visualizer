namespace Tgm3Visualizer.ViewModels;

/// <summary>
/// Interface for ViewModels that display common level/time info (LevelTimeCard).
/// </summary>
public interface ICommonLevelTimeViewModel
{
    int Level { get; set; }
    string FormattedTime { get; set; }
}
