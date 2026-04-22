using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Tgm3Visualizer.Brushes;

namespace Tgm3Visualizer.Controls.Master;

public sealed partial class InternalGradeCard : UserControl
{
    private static readonly SolidColorBrush GreenBrush = new(Colors.LimeGreen);
    private static readonly SolidColorBrush WhiteBrush = new(Colors.White);

    public string InternalGradeText
    {
        get => (string)GetValue(InternalGradeTextProperty);
        set => SetValue(InternalGradeTextProperty, value);
    }

    public static readonly DependencyProperty InternalGradeTextProperty =
        DependencyProperty.Register(nameof(InternalGradeText), typeof(string),
                                    typeof(InternalGradeCard), new PropertyMetadata("0 (Grade 9)"));

    public bool IsInternalGradeQualified
    {
        get => (bool)GetValue(IsInternalGradeQualifiedProperty);
        set => SetValue(IsInternalGradeQualifiedProperty, value);
    }

    public static readonly DependencyProperty IsInternalGradeQualifiedProperty =
        DependencyProperty.Register(nameof(IsInternalGradeQualified), typeof(bool),
                                    typeof(InternalGradeCard), new PropertyMetadata(false, OnQualificationChanged));

    public bool IsMaxInternalGrade
    {
        get => (bool)GetValue(IsMaxInternalGradeProperty);
        set => SetValue(IsMaxInternalGradeProperty, value);
    }

    public static readonly DependencyProperty IsMaxInternalGradeProperty =
        DependencyProperty.Register(nameof(IsMaxInternalGrade), typeof(bool),
                                    typeof(InternalGradeCard), new PropertyMetadata(false, OnQualificationChanged));

    private static void OnQualificationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var card = (InternalGradeCard)d;
        card.UpdateInternalGradeForeground();
    }

    private void UpdateInternalGradeForeground()
    {
        if (IsMaxInternalGrade)
            InternalGradeTextBlock.Foreground = RainbowBrushes.Rainbow;
        else if (IsInternalGradeQualified)
            InternalGradeTextBlock.Foreground = GreenBrush;
        else
            InternalGradeTextBlock.Foreground = WhiteBrush;
    }

    public string StaffRollGradePointsDisplay
    {
        get => (string)GetValue(StaffRollGradePointsDisplayProperty);
        set => SetValue(StaffRollGradePointsDisplayProperty, value);
    }

    public static readonly DependencyProperty StaffRollGradePointsDisplayProperty =
        DependencyProperty.Register(nameof(StaffRollGradePointsDisplay), typeof(string),
                                    typeof(InternalGradeCard), new PropertyMetadata("+0"));

    public int AwardedGradePoints
    {
        get => (int)GetValue(AwardedGradePointsProperty);
        set => SetValue(AwardedGradePointsProperty, value);
    }

    public static readonly DependencyProperty AwardedGradePointsProperty =
        DependencyProperty.Register(nameof(AwardedGradePoints), typeof(int),
                                    typeof(InternalGradeCard), new PropertyMetadata(0));

    public double AwardedGradePointsProgress
    {
        get => (double)GetValue(AwardedGradePointsProgressProperty);
        set => SetValue(AwardedGradePointsProgressProperty, value);
    }

    public static readonly DependencyProperty AwardedGradePointsProgressProperty =
        DependencyProperty.Register(nameof(AwardedGradePointsProgress), typeof(double),
                                    typeof(InternalGradeCard), new PropertyMetadata(0.0));

    public string DecayTime
    {
        get => (string)GetValue(DecayTimeProperty);
        set => SetValue(DecayTimeProperty, value);
    }

    public static readonly DependencyProperty DecayTimeProperty =
        DependencyProperty.Register(nameof(DecayTime), typeof(string),
                                    typeof(InternalGradeCard), new PropertyMetadata("0 Frame"));

    public double DecayTimeProgress
    {
        get => (double)GetValue(DecayTimeProgressProperty);
        set => SetValue(DecayTimeProgressProperty, value);
    }

    public static readonly DependencyProperty DecayTimeProgressProperty =
        DependencyProperty.Register(nameof(DecayTimeProgress), typeof(double),
                                    typeof(InternalGradeCard), new PropertyMetadata(0.0));

    public InternalGradeCard()
    {
        InitializeComponent();
    }
}
