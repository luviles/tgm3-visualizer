using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Tgm3Visualizer.Brushes;

namespace Tgm3Visualizer.Converters;

public class CoolRainbowToBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush WhiteBrush = new(Microsoft.UI.Colors.White);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isHighlighted)
        {
            return isHighlighted
                ? RainbowBrushes.Rainbow
                : WhiteBrush;
        }
        return WhiteBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
