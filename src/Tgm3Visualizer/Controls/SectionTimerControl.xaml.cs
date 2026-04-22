using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Controls;

public sealed partial class SectionTimerControl : UserControl
{
    public ObservableCollection<SectionInfo> Sections
    {
        get => (ObservableCollection<SectionInfo>)GetValue(SectionsProperty);
        set => SetValue(SectionsProperty, value);
    }

    public static readonly DependencyProperty SectionsProperty =
        DependencyProperty.Register(nameof(Sections), typeof(ObservableCollection<SectionInfo>),
                                    typeof(SectionTimerControl), new PropertyMetadata(null));

    public SectionTimerControl()
    {
        InitializeComponent();
    }
}
