using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Easy;

public sealed partial class HanabiScoreCard : UserControl
{
    public int HanabiScoreValue
    {
        get => (int)GetValue(HanabiScoreValueProperty);
        set => SetValue(HanabiScoreValueProperty, value);
    }

    public static readonly DependencyProperty HanabiScoreValueProperty =
        DependencyProperty.Register(nameof(HanabiScoreValue), typeof(int),
                                    typeof(HanabiScoreCard), new PropertyMetadata(0));

    public HanabiScoreCard()
    {
        InitializeComponent();
    }
}
