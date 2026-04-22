using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Tgm3Visualizer.ViewModels;

namespace Tgm3Visualizer.Views;

public sealed partial class MasterModeView : UserControl
{
    public MasterModeViewModel ViewModel { get; }

    public MasterModeView()
    {
        InitializeComponent();
        ViewModel = (App.Current as App)?.Services.GetRequiredService<MasterModeViewModel>()
                    ?? throw new InvalidOperationException("MasterModeViewModel not registered in DI");
    }
}
