using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Tgm3Visualizer.Controls.Sakura;

public sealed partial class StageCard : UserControl
{
    public string Stage
    {
        get => (string)GetValue(StageProperty);
        set => SetValue(StageProperty, value);
    }

    public int ClearedStages
    {
        get => (int)GetValue(ClearedStagesProperty);
        set => SetValue(ClearedStagesProperty, value);
    }

    public int JewelCount
    {
        get => (int)GetValue(JewelCountProperty);
        set => SetValue(JewelCountProperty, value);
    }

    public static readonly DependencyProperty StageProperty =
        DependencyProperty.Register(nameof(Stage), typeof(string), typeof(StageCard), new PropertyMetadata("1"));

    public static readonly DependencyProperty ClearedStagesProperty =
        DependencyProperty.Register(nameof(ClearedStages), typeof(int), typeof(StageCard), new PropertyMetadata(0));

    public static readonly DependencyProperty JewelCountProperty =
        DependencyProperty.Register(nameof(JewelCount), typeof(int), typeof(StageCard), new PropertyMetadata(0));

    public Brush ClearedForeground
    {
        get => (Brush)GetValue(ClearedForegroundProperty);
        set => SetValue(ClearedForegroundProperty, value);
    }

    public static readonly DependencyProperty ClearedForegroundProperty =
        DependencyProperty.Register(nameof(ClearedForeground), typeof(Brush), typeof(StageCard),
            new PropertyMetadata(new SolidColorBrush(Microsoft.UI.Colors.White)));

    public StageCard()
    {
        InitializeComponent();
    }
}
