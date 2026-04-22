using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Master;

public sealed partial class GradeCoolRegretCard : UserControl
{
    public string Grade
    {
        get => (string)GetValue(GradeProperty);
        set => SetValue(GradeProperty, value);
    }

    public static readonly DependencyProperty GradeProperty =
        DependencyProperty.Register(nameof(Grade), typeof(string),
                                    typeof(GradeCoolRegretCard), new PropertyMetadata("--"));

    public int CoolDisplay
    {
        get => (int)GetValue(CoolDisplayProperty);
        set => SetValue(CoolDisplayProperty, value);
    }

    public static readonly DependencyProperty CoolDisplayProperty =
        DependencyProperty.Register(nameof(CoolDisplay), typeof(int),
                                    typeof(GradeCoolRegretCard), new PropertyMetadata(0));

    public bool IsSectionCoolQualified
    {
        get => (bool)GetValue(IsSectionCoolQualifiedProperty);
        set => SetValue(IsSectionCoolQualifiedProperty, value);
    }

    public static readonly DependencyProperty IsSectionCoolQualifiedProperty =
        DependencyProperty.Register(nameof(IsSectionCoolQualified), typeof(bool),
                                    typeof(GradeCoolRegretCard), new PropertyMetadata(false));

    public int SectionRegretCount
    {
        get => (int)GetValue(SectionRegretCountProperty);
        set => SetValue(SectionRegretCountProperty, value);
    }

    public static readonly DependencyProperty SectionRegretCountProperty =
        DependencyProperty.Register(nameof(SectionRegretCount), typeof(int),
                                    typeof(GradeCoolRegretCard), new PropertyMetadata(0));

    public GradeCoolRegretCard()
    {
        InitializeComponent();
    }
}
