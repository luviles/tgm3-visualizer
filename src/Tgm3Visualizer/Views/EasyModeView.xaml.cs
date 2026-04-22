using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Views;

public sealed partial class EasyModeView : UserControl
{
    public EasyModeViewModel ViewModel { get; }

    public EasyModeView()
    {
        InitializeComponent();
        ViewModel = (App.Current as App)?.Services.GetRequiredService<EasyModeViewModel>()
                    ?? throw new InvalidOperationException("EasyModeViewModel not registered in DI");
    }
}
