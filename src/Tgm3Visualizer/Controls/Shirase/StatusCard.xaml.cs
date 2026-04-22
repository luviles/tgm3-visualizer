using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Shirase;

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

    public string GarbageQuotaText
    {
        get => (string)GetValue(GarbageQuotaTextProperty);
        set => SetValue(GarbageQuotaTextProperty, value);
    }

    public static readonly DependencyProperty GarbageQuotaTextProperty =
        DependencyProperty.Register(nameof(GarbageQuotaText), typeof(string),
                                    typeof(StatusCard), new PropertyMetadata(""));

    public double GarbageQuotaValue
    {
        get => (double)GetValue(GarbageQuotaValueProperty);
        set => SetValue(GarbageQuotaValueProperty, value);
    }

    public static readonly DependencyProperty GarbageQuotaValueProperty =
        DependencyProperty.Register(nameof(GarbageQuotaValue), typeof(double),
                                    typeof(StatusCard), new PropertyMetadata(0.0));

    public double GarbageQuotaMax
    {
        get => (double)GetValue(GarbageQuotaMaxProperty);
        set => SetValue(GarbageQuotaMaxProperty, value);
    }

    public static readonly DependencyProperty GarbageQuotaMaxProperty =
        DependencyProperty.Register(nameof(GarbageQuotaMax), typeof(double),
                                    typeof(StatusCard), new PropertyMetadata(1.0));

    public bool ShowGarbageQuota
    {
        get => (bool)GetValue(ShowGarbageQuotaProperty);
        set => SetValue(ShowGarbageQuotaProperty, value);
    }

    public static readonly DependencyProperty ShowGarbageQuotaProperty =
        DependencyProperty.Register(nameof(ShowGarbageQuota), typeof(bool),
                                    typeof(StatusCard), new PropertyMetadata(false));

    public StatusCard()
    {
        InitializeComponent();
    }
}
