using System;

namespace Tgm3Visualizer.Core.Interfaces;

/// <summary>
/// Interface for reading game memory
/// </summary>
public interface IMemoryReader
{
    /// <summary>
    /// Check if the game process is attached and running
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Attempt to attach to the game process
    /// </summary>
    /// <returns>True if successfully attached</returns>
    bool TryConnect();

    /// <summary>
    /// Read a 16-bit unsigned integer from memory
    /// </summary>
    bool TryReadUInt16(IntPtr address, out ushort value);

    /// <summary>
    /// Read a 32-bit signed integer from memory
    /// </summary>
    bool TryReadInt32(IntPtr address, out int value);

    /// <summary>
    /// Read bytes from memory
    /// </summary>
    bool TryReadBytes(IntPtr address, int size, out byte[] bytes);

    /// <summary>
    /// Get the base address of the game module (game.exe)
    /// </summary>
    /// <returns>Base address of the module, or IntPtr.Zero if not connected</returns>
    IntPtr GetModuleBaseAddress();
}
