using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Controls.Shirase;

public sealed partial class MenuDescription : UserControl
{
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
