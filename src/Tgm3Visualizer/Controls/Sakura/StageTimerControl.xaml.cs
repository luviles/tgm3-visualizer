using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Controls.Sakura;

public sealed partial class StageTimerControl : UserControl
{
    public ObservableCollection<SectionInfo> Sections
    {
        get => (ObservableCollection<SectionInfo>)GetValue(SectionsProperty);
        set => SetValue(SectionsProperty, value);
    }

    public static readonly DependencyProperty SectionsProperty =
        DependencyProperty.Register(nameof(Sections), typeof(ObservableCollection<SectionInfo>),
                                    typeof(StageTimerControl), new PropertyMetadata(null));

    public StageTimerControl()
    {
        InitializeComponent();
    }
}
