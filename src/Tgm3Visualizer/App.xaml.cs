using System;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Tgm3Visualizer.Core.Interfaces;
using Tgm3Visualizer.Services;
using Tgm3Visualizer.ViewModels;
using Tgm3Visualizer.Views;

namespace Tgm3Visualizer;

public partial class App : Application
{
    private Window? _window;
    public IServiceProvider Services { get; }
    private static string LogFile = string.Empty;

    public App()
    {
        Services = ConfigureServices();
        InitializeComponent();

        // Initialize log file
        LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "tgm3_debug.log");
        File.WriteAllText(LogFile, $"=== TGM3 Visualizer Log Started at {DateTime.Now} ===\n");
    }

    public static void Log(string message)
    {
        var logLine = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
        try
        {
            File.AppendAllText(LogFile, logLine + "\n");
        }
        catch { }
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Services - register as IMemoryReader interface
        services.AddSingleton<IMemoryReader, MemoryService>();
        services.AddSingleton<GameDataService>();
        services.AddSingleton<SettingsService>();

        // ViewModels
        services.AddTransient<MasterModeViewModel>();
        services.AddTransient<ShiraseModeViewModel>();
        services.AddTransient<SakuraModeViewModel>();
        services.AddTransient<EasyModeViewModel>();

        return services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();

        // Start timer
        var gameDataService = Services.GetRequiredService<GameDataService>();
        gameDataService.Start();
        Log("App.OnLaunched - GameDataService.Start() called");
    }
}
