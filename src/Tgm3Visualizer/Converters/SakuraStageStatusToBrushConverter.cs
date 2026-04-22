using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Converters;

/// <summary>
/// Converts SectionStatus to Brush for Sakura stage timer:
/// None = dim, Normal = White (in progress), Cool = LimeGreen (cleared), Regret = Crimson (forfeited)
/// </summary>
public class SakuraStageStatusToBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush DimBrush = new(Microsoft.UI.ColorHelper.FromArgb(255, 0x55, 0x66, 0x77));
    private static readonly SolidColorBrush WhiteBrush = new(Colors.White);
    private static readonly SolidColorBrush GreenBrush = new(Colors.LimeGreen);
    private static readonly SolidColorBrush RedBrush = new(Colors.Crimson);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is SectionStatus status)
        {
            return status switch
            {
                SectionStatus.Cool => GreenBrush,
                SectionStatus.Regret => RedBrush,
                SectionStatus.Normal => WhiteBrush,
                _ => DimBrush
            };
        }
        return DimBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
