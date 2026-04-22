namespace Tgm3Visualizer.ViewModels;

/// <summary>
/// Common interface for status card display properties
/// </summary>
public interface ICommonStatusCardViewModel
{
    string StatusCardTitle { get; set; }
    string StatusCardSubtitle { get; set; }
    bool ShowStaffRollTime { get; set; }
}
