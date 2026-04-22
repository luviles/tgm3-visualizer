using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Sakura;

public sealed partial class LimitTimeCard : UserControl
{
    public string LimitTime
    {
        get => (string)GetValue(LimitTimeProperty);
        set => SetValue(LimitTimeProperty, value);
    }

    public static readonly DependencyProperty LimitTimeProperty =
        DependencyProperty.Register(nameof(LimitTime), typeof(string), typeof(LimitTimeCard), new PropertyMetadata("00:00:00"));

    public LimitTimeCard()
    {
        InitializeComponent();
    }
}
