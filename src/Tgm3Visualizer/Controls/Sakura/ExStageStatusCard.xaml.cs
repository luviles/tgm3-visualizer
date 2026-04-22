using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Tgm3Visualizer.Brushes;

namespace Tgm3Visualizer.Controls.Sakura;

public sealed partial class ExStageStatusCard : UserControl
{
    private static readonly SolidColorBrush GreenBrush = new(Colors.LimeGreen);
    private static readonly SolidColorBrush DimBrush = new(ColorHelper.FromArgb(255, 0x55, 0x66, 0x77));

    public int ExStageTier
    {
        get => (int)GetValue(ExStageTierProperty);
        set => SetValue(ExStageTierProperty, value);
    }

    public static readonly DependencyProperty ExStageTierProperty =
        DependencyProperty.Register(nameof(ExStageTier), typeof(int), typeof(ExStageStatusCard),
            new PropertyMetadata(0, OnExStageTierChanged));

    private static void OnExStageTierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (ExStageStatusCard)d;
        int tier = (int)e.NewValue;
        card.Ex3Text.Foreground = tier == 3 ? GreenBrush : DimBrush;
        card.Ex5Text.Foreground = tier == 5 ? GreenBrush : DimBrush;
        card.Ex7Text.Foreground = tier == 7 ? RainbowBrushes.Rainbow : DimBrush;
    }

    public ExStageStatusCard()
    {
        InitializeComponent();
    }
}
