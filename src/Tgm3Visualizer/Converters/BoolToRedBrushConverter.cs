using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace Tgm3Visualizer.Converters;

/// <summary>
/// Converts bool to Brush: true = Crimson, false = White
/// Used for time limit exceeded highlighting
/// </summary>
public class BoolToRedBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush RedBrush = new SolidColorBrush(Colors.Crimson);
    private static readonly SolidColorBrush WhiteBrush = new SolidColorBrush(Colors.White);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isExceeded)
            return isExceeded ? RedBrush : WhiteBrush;
        return WhiteBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
