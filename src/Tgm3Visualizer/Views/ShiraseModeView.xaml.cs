using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Views;

public sealed partial class ShiraseModeView : UserControl
{
    public ShiraseModeViewModel ViewModel { get; }

    public ShiraseModeView()
    {
        InitializeComponent();
        ViewModel = (App.Current as App)?.Services.GetRequiredService<ShiraseModeViewModel>()
                    ?? throw new InvalidOperationException("ShiraseModeViewModel not registered in DI");
    }
}
