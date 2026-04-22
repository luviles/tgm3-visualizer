using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Converters;

public class SectionStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is SectionStatus status)
        {
            return status switch
            {
                SectionStatus.Cool => new SolidColorBrush(Colors.LimeGreen),
                SectionStatus.Regret => new SolidColorBrush(Colors.Crimson),
                SectionStatus.Normal => new SolidColorBrush(Colors.White),
                _ => new SolidColorBrush(Colors.Gray)
            };
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
