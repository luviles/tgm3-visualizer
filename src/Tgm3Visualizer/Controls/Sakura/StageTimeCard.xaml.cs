using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Sakura;

public sealed partial class StageTimeCard : UserControl
{
    public string StageTime
    {
        get => (string)GetValue(StageTimeProperty);
        set => SetValue(StageTimeProperty, value);
    }

    public static readonly DependencyProperty StageTimeProperty =
        DependencyProperty.Register(nameof(StageTime), typeof(string), typeof(StageTimeCard), new PropertyMetadata(""));

    public StageTimeCard()
    {
        InitializeComponent();
    }
}
