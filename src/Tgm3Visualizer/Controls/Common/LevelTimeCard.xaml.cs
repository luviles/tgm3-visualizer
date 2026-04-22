using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Common;

public sealed partial class LevelTimeCard : UserControl
{
    public int Level
    {
        get => (int)GetValue(LevelProperty);
        set => SetValue(LevelProperty, value);
    }

    public string FormattedTime
    {
        get => (string)GetValue(FormattedTimeProperty);
        set => SetValue(FormattedTimeProperty, value);
    }

    public static readonly DependencyProperty LevelProperty =
        DependencyProperty.Register(nameof(Level), typeof(int), typeof(LevelTimeCard), new PropertyMetadata(0));

    public static readonly DependencyProperty FormattedTimeProperty =
        DependencyProperty.Register(nameof(FormattedTime), typeof(string), typeof(LevelTimeCard), new PropertyMetadata("00:00:00"));

    public LevelTimeCard()
    {
        InitializeComponent();
    }
}
