using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Shirase;

public sealed partial class ThresholdCard : UserControl
{
    public string RegretDeadline
    {
        get => (string)GetValue(RegretDeadlineProperty);
        set => SetValue(RegretDeadlineProperty, value);
    }

    public static readonly DependencyProperty RegretDeadlineProperty =
        DependencyProperty.Register(nameof(RegretDeadline), typeof(string),
                                    typeof(ThresholdCard), new PropertyMetadata("N/A"));

    public string Level500TimeLimit
    {
        get => (string)GetValue(Level500TimeLimitProperty);
        set => SetValue(Level500TimeLimitProperty, value);
    }

    public static readonly DependencyProperty Level500TimeLimitProperty =
        DependencyProperty.Register(nameof(Level500TimeLimit), typeof(string),
                                    typeof(ThresholdCard), new PropertyMetadata("03:03:00"));

    public string Level1000TimeLimit
    {
        get => (string)GetValue(Level1000TimeLimitProperty);
        set => SetValue(Level1000TimeLimitProperty, value);
    }

    public static readonly DependencyProperty Level1000TimeLimitProperty =
        DependencyProperty.Register(nameof(Level1000TimeLimit), typeof(string),
                                    typeof(ThresholdCard), new PropertyMetadata("06:06:00"));

    public bool IsLevel500Exceeded
    {
        get => (bool)GetValue(IsLevel500ExceededProperty);
        set => SetValue(IsLevel500ExceededProperty, value);
    }

    public static readonly DependencyProperty IsLevel500ExceededProperty =
        DependencyProperty.Register(nameof(IsLevel500Exceeded), typeof(bool),
                                    typeof(ThresholdCard), new PropertyMetadata(false));

    public bool IsLevel1000Exceeded
    {
        get => (bool)GetValue(IsLevel1000ExceededProperty);
        set => SetValue(IsLevel1000ExceededProperty, value);
    }

    public static readonly DependencyProperty IsLevel1000ExceededProperty =
        DependencyProperty.Register(nameof(IsLevel1000Exceeded), typeof(bool),
                                    typeof(ThresholdCard), new PropertyMetadata(false));

    public ThresholdCard()
    {
        InitializeComponent();
    }
}
