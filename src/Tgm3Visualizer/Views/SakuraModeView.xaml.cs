using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Views;

public sealed partial class SakuraModeView : UserControl
{
    public SakuraModeViewModel ViewModel { get; }

    public SakuraModeView()
    {
        InitializeComponent();
        ViewModel = (App.Current as App)?.Services.GetRequiredService<SakuraModeViewModel>()
                    ?? throw new InvalidOperationException("SakuraModeViewModel not registered in DI");
    }
}
