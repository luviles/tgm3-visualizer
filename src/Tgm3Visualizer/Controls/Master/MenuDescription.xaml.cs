using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Controls.Master;

public sealed partial class MenuDescription : UserControl
{
    public ObservableCollection<GradeHistoryItem> GradeHistoryItems
    {
        get => (ObservableCollection<GradeHistoryItem>)GetValue(GradeHistoryItemsProperty);
        set => SetValue(GradeHistoryItemsProperty, value);
    }

    public static readonly DependencyProperty GradeHistoryItemsProperty =
        DependencyProperty.Register(nameof(GradeHistoryItems), typeof(ObservableCollection<GradeHistoryItem>),
                                    typeof(MenuDescription), new PropertyMetadata("N/A"));

    public string AverageGrade
    {
        get => (string)GetValue(AverageGradeProperty);
        set => SetValue(AverageGradeProperty, value);
    }

    public static readonly DependencyProperty AverageGradeProperty =
        DependencyProperty.Register(nameof(AverageGrade), typeof(string),
                                    typeof(MenuDescription), new PropertyMetadata("N/A"));

    public string DemotionProgress
    {
        get => (string)GetValue(DemotionProgressProperty);
        set => SetValue(DemotionProgressProperty, value);
    }

    public static readonly DependencyProperty DemotionProgressProperty =
        DependencyProperty.Register(nameof(DemotionProgress), typeof(string),
                                    typeof(MenuDescription), new PropertyMetadata("0/30"));

    public string ControlMode
    {
        get => (string)GetValue(ControlModeProperty);
        set => SetValue(ControlModeProperty, value);
    }

    public static readonly DependencyProperty ControlModeProperty =
        DependencyProperty.Register(nameof(ControlMode), typeof(string),
                                    typeof(MenuDescription), new PropertyMetadata(""));

    public MenuDescription()
    {
        InitializeComponent();
    }
}
