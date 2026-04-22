using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace Tgm3Visualizer.Brushes;

public static class RainbowBrushes
{
    public static Brush Rainbow { get; } = CreateRainbowBrush();

    private static Brush CreateRainbowBrush()
    {
        var brush = new LinearGradientBrush
        {
            StartPoint = new Point(0.5, 0),
            EndPoint = new Point(0.5, 1)
        };

        brush.GradientStops.Add(new GradientStop { Color = ColorHelper.FromArgb(255, 108, 224, 245), Offset = 0.00 });
        brush.GradientStops.Add(new GradientStop { Color = ColorHelper.FromArgb(255, 67, 255, 171), Offset = 0.18 });
        brush.GradientStops.Add(new GradientStop { Color = ColorHelper.FromArgb(255, 151, 255, 6), Offset = 0.30 });
        brush.GradientStops.Add(new GradientStop { Color = ColorHelper.FromArgb(255, 255, 255, 0), Offset = 0.39 });
        brush.GradientStops.Add(new GradientStop { Color = ColorHelper.FromArgb(255, 255, 199, 155), Offset = 0.51 });
        brush.GradientStops.Add(new GradientStop { Color = ColorHelper.FromArgb(255, 255, 107, 160), Offset = 0.70 });
        brush.GradientStops.Add(new GradientStop { Color = ColorHelper.FromArgb(255, 249, 125, 198), Offset = 0.86 });
        brush.GradientStops.Add(new GradientStop { Color = ColorHelper.FromArgb(255, 131, 97, 165), Offset = 1.00 });

        return brush;
    }
}
