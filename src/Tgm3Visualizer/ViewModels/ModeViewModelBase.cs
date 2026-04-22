using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Tgm3Visualizer.Core.Models;
using Tgm3Visualizer.Services;

namespace Tgm3Visualizer.ViewModels;

/// <summary>
/// Base class for mode ViewModels providing common GameDataService subscription,
/// ConnectionStatus, and GameMode management.
/// </summary>
public abstract partial class ModeViewModelBase : ObservableObject
{
    protected readonly GameDataService _gameDataService;

    [ObservableProperty] private string _connectionStatus = "Not connected";
    [ObservableProperty] private string _gameMode = "Waiting for game...";
    [ObservableProperty] private bool _isActive;

    protected ModeViewModelBase(GameDataService gameDataService)
    {
        _gameDataService = gameDataService;
        _gameDataService.PropertyChanged += OnGameDataServicePropertyChanged;
        ConnectionStatus = _gameDataService.IsGameRunning ? "Connected" : "Not connected";
    }

    private void OnGameDataServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GameDataService.IsGameRunning))
        {
            ConnectionStatus = _gameDataService.IsGameRunning ? "Connected" : "Not connected";
            if (!_gameDataService.IsGameRunning)
            {
                GameMode = "Waiting for game...";
            }
        }
        else if (e.PropertyName == nameof(GameDataService.CurrentState) && IsActive)
        {
            UpdateFromGameState(_gameDataService.CurrentState);
        }
    }

    public abstract void UpdateFromGameState(ProcessedGameState state);
}
