using CommunityToolkit.Mvvm.ComponentModel;

namespace Tgm3Visualizer.ViewModels;

public partial class SectionInfo : ObservableObject
{
    [ObservableProperty] private string _range = "";
    [ObservableProperty] private string _time = "";
    [ObservableProperty] private string _seventyPercentTime = "";
    [ObservableProperty] private string _cumulativeTime = "";
    [ObservableProperty] private SectionStatus _status;
    [ObservableProperty] private bool _isCool;
}

public enum SectionStatus
{
    None,
    Normal,
    Cool,
    Regret
}

public partial class GradeHistoryItem : ObservableObject
{
    [ObservableProperty] private string _gradeName = "";
    [ObservableProperty] private bool _isHighlighted;
}
