using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Easy;

public sealed partial class StatusCard : UserControl
{
    public string StatusCardTitle
    {
        get => (string)GetValue(StatusCardTitleProperty);
        set => SetValue(StatusCardTitleProperty, value);
    }

    public static readonly DependencyProperty StatusCardTitleProperty =
        DependencyProperty.Register(nameof(StatusCardTitle), typeof(string),
                                    typeof(StatusCard), new PropertyMetadata("NORMAL\nPLAY"));

    public string StatusCardSubtitle
    {
        get => (string)GetValue(StatusCardSubtitleProperty);
        set => SetValue(StatusCardSubtitleProperty, value);
    }

    public static readonly DependencyProperty StatusCardSubtitleProperty =
        DependencyProperty.Register(nameof(StatusCardSubtitle), typeof(string),
                                    typeof(StatusCard), new PropertyMetadata(""));

    public bool ShowStaffRollTime
    {
        get => (bool)GetValue(ShowStaffRollTimeProperty);
        set => SetValue(ShowStaffRollTimeProperty, value);
    }

    public static readonly DependencyProperty ShowStaffRollTimeProperty =
        DependencyProperty.Register(nameof(ShowStaffRollTime), typeof(bool),
                                    typeof(StatusCard), new PropertyMetadata(false));

    public StatusCard()
    {
        InitializeComponent();
    }
}
