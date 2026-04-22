using Tgm3Visualizer.Core.Models;

namespace Tgm3Visualizer.Core.Interfaces;

/// <summary>
/// Interface for processing game state changes
/// </summary>
public interface IGameStateProcessor
{
    /// <summary>
    /// Process updated game state
    /// </summary>
    /// <param name="currentState">Current game state from memory</param>
    /// <returns>Processed game state with calculated values</returns>
    ProcessedGameState Process(GameState currentState);

    /// <summary>
    /// Reset processor state (new game)
    /// </summary>
    void Reset();
}
