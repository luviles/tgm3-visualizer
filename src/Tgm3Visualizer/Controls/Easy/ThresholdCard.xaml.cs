using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Easy;

public sealed partial class ThresholdCard : UserControl
{
    public string RegretDeadline
    {
        get => (string)GetValue(RegretDeadlineProperty);
        set => SetValue(RegretDeadlineProperty, value);
    }

    public static readonly DependencyProperty RegretDeadlineProperty =
        DependencyProperty.Register(nameof(RegretDeadline), typeof(string),
                                    typeof(ThresholdCard), new PropertyMetadata("N/A"));

    public ThresholdCard()
    {
        InitializeComponent();
    }
}
