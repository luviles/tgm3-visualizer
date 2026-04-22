using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Easy;

public sealed partial class GradeRegretCard : UserControl
{
    public string Grade
    {
        get => (string)GetValue(GradeProperty);
        set => SetValue(GradeProperty, value);
    }

    public static readonly DependencyProperty GradeProperty =
        DependencyProperty.Register(nameof(Grade), typeof(string),
                                    typeof(GradeRegretCard), new PropertyMetadata("--"));

    public int SectionRegretCount
    {
        get => (int)GetValue(SectionRegretCountProperty);
        set => SetValue(SectionRegretCountProperty, value);
    }

    public static readonly DependencyProperty SectionRegretCountProperty =
        DependencyProperty.Register(nameof(SectionRegretCount), typeof(int),
                                    typeof(GradeRegretCard), new PropertyMetadata(0));

    public GradeRegretCard()
    {
        InitializeComponent();
    }
}
