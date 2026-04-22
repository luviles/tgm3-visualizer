using Microsoft.UI.Xaml.Controls;

namespace Tgm3Visualizer.Views;

/// <summary>
/// Placeholder view displayed when not actively playing (GameProcessStatus != 3)
/// </summary>
public sealed partial class OverlayView : UserControl
{
    public OverlayView()
    {
        InitializeComponent();
    }
}
