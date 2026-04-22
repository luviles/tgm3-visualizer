using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Tgm3Visualizer.Core.Models;
using Tgm3Visualizer.Services;
using Microsoft.UI.Windowing;
using Tgm3Visualizer.Views;

namespace Tgm3Visualizer;

public sealed partial class MainWindow : Window
{
    private readonly GameDataService _gameDataService;
    private GameType _currentGameType = GameType.None;
    private int _currentGameProcessStatus = -1;

    // Pre-loaded mode views
    private readonly EasyModeView _easyView;
    private readonly MasterModeView _masterView;
    private readonly ShiraseModeView _shiraseView;
    private readonly SakuraModeView _sakuraView;
    private readonly OverlayView _overlayView;

    public MainWindow()
    {
        InitializeComponent();
        Title = "TGM3 Visualizer";

        // Fixed window size 480x1080, disable resizing
        var appWindow = this.AppWindow;
        appWindow.Resize(new Windows.Graphics.SizeInt32(480, 1080));
        appWindow.SetIcon("Assets\\app.ico");
        var presenter = (OverlappedPresenter)appWindow.Presenter;
        presenter.IsResizable = false;

        // Dark title bar
        var titleBar = AppWindow.TitleBar;
        titleBar.BackgroundColor = new Windows.UI.Color { R = 0x1A, G = 0x1A, B = 0x2E, A = 0xFF };
        titleBar.ForegroundColor = Colors.White;
        titleBar.InactiveBackgroundColor = new Windows.UI.Color { R = 0x1A, G = 0x1A, B = 0x2E, A = 0xFF };
        titleBar.InactiveForegroundColor = Colors.Gray;
        titleBar.ButtonBackgroundColor = new Windows.UI.Color { R = 0x1A, G = 0x1A, B = 0x2E, A = 0xFF };
        titleBar.ButtonForegroundColor = Colors.White;

        // Pre-load all mode views
        _overlayView = new OverlayView();
        _easyView = new EasyModeView();
        _masterView = new MasterModeView();
        _shiraseView = new ShiraseModeView();
        _sakuraView = new SakuraModeView();

        MainGrid.Children.Add(_overlayView);
        MainGrid.Children.Add(_easyView);
        MainGrid.Children.Add(_masterView);
        MainGrid.Children.Add(_shiraseView);
        MainGrid.Children.Add(_sakuraView);

        HideAllViews();
        _overlayView.Visibility = Visibility.Visible;

        // Get GameDataService from DI
        _gameDataService = (App.Current as App)?.Services.GetRequiredService<GameDataService>()
                           ?? throw new InvalidOperationException("GameDataService not registered");

        // Subscribe to property changes
        _gameDataService.PropertyChanged += OnGameDataServicePropertyChanged;
    }

    private void OnGameDataServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GameDataService.CurrentState))
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                UpdateViewIfNeeded();
            });
        }
    }

    private void HideAllViews()
    {
        _overlayView.Visibility = Visibility.Collapsed;
        _easyView.Visibility = Visibility.Collapsed;
        _masterView.Visibility = Visibility.Collapsed;
        _shiraseView.Visibility = Visibility.Collapsed;
        _sakuraView.Visibility = Visibility.Collapsed;

        _easyView.ViewModel.IsActive = false;
        _masterView.ViewModel.IsActive = false;
        _shiraseView.ViewModel.IsActive = false;
        _sakuraView.ViewModel.IsActive = false;
    }

    private void UpdateViewIfNeeded()
    {
        var state = _gameDataService.CurrentState;
        var newGameType = state.GameType;
        var newGameProcessStatus = state.GameProcessStatus;

        // First time game stops: switch to blank overlay view
        if (_currentGameProcessStatus == 3 && newGameProcessStatus != 3)
        {
            ShowOverlayView();
            return;
        }

        // Ignore when switching to Demo Process Status (2)
        if (newGameProcessStatus != 3) {
            return;
        }

        // !GameType.None → GameType.None: keep previous view (waiting screen)
        if (_currentGameType != GameType.None && newGameType == GameType.None)
        {
            return;
        }

        // Doubles not supported: keep previous view
        if (newGameType == GameType.Doubles)
        {
            return;
        }

        // Qualified Master: keep previous view
        // Master → Qualified Master transition is handled by keeping the view
        if (newGameType == GameType.Qualified_Master)
        {
            return;
        }

        // Same game type: no change needed
        if (newGameType == _currentGameType)
        {
            return;
        }

        // Select new view (toggle Visibility)
        _currentGameType = newGameType;
        _currentGameProcessStatus = newGameProcessStatus;

        HideAllViews();
        switch (newGameType)
        {
            case GameType.Easy:
                _easyView.ViewModel.IsActive = true;
                _easyView.Visibility = Visibility.Visible;
                break;
            case GameType.Master:
                _masterView.ViewModel.IsActive = true;
                _masterView.Visibility = Visibility.Visible;
                break;
            case GameType.Shirase:
                _shiraseView.ViewModel.IsActive = true;
                _shiraseView.Visibility = Visibility.Visible;
                break;
            case GameType.Sakura:
                _sakuraView.ViewModel.IsActive = true;
                _sakuraView.Visibility = Visibility.Visible;
                break;
        }
    }

    private void ShowOverlayView()
    {
        _currentGameType = GameType.None;
        _currentGameProcessStatus = -1;
        HideAllViews();
        _overlayView.Visibility = Visibility.Visible;
    }
}
