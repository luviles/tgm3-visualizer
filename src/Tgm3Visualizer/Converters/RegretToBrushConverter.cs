using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Converters;

/// <summary>
/// Converts SectionStatus to brush: Crimson if Regret, White otherwise
/// Used for Section Time column coloring (Cool does not affect color)
/// </summary>
public class RegretToBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush RegretBrush = new SolidColorBrush(Colors.Crimson);
    private static readonly SolidColorBrush NormalBrush = new SolidColorBrush(Colors.White);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is SectionStatus status)
            return status == SectionStatus.Regret ? RegretBrush : NormalBrush;
        return NormalBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
