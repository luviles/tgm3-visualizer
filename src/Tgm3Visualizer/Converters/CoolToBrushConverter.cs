using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace Tgm3Visualizer.Converters;

/// <summary>
/// Converts IsCool bool to brush: LimeGreen if true, White if false
/// Used for 70% Time column coloring
/// </summary>
public class CoolToBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush CoolBrush = new SolidColorBrush(Colors.LimeGreen);
    private static readonly SolidColorBrush NormalBrush = new SolidColorBrush(Colors.White);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isCool)
            return isCool ? CoolBrush : NormalBrush;
        return NormalBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
