using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tgm3Visualizer.Core.Interfaces;

namespace Tgm3Visualizer.Services;

/// <summary>
/// Memory reading service using Windows API P/Invoke
/// Compatible with .NET 8 without external NuGet dependencies
/// </summary>
public class MemoryService : IMemoryReader
{
    private Process? _gameProcess;
    private IntPtr _processHandle;
    private bool _isConnected;

    public bool IsConnected => _isConnected && _gameProcess != null && !_gameProcess.HasExited;

    public bool TryConnect()
    {
        // Clean up existing connection
        if (_processHandle != IntPtr.Zero)
        {
            CloseHandle(_processHandle);
            _processHandle = IntPtr.Zero;
        }

        var processes = Process.GetProcessesByName("game");

        if (processes.Length == 0)
        {
            _isConnected = false;
            return false;
        }

        _gameProcess = processes[0];
        Debug.WriteLine($"[MemoryService] Process ID: {_gameProcess.Id}, Name: {_gameProcess.ProcessName}");

        try
        {
            // PROCESS_VM_READ (0x0010) | PROCESS_QUERY_INFORMATION (0x0400)
            _processHandle = OpenProcess(0x0410, false, _gameProcess.Id);

            var error = Marshal.GetLastWin32Error();
            Debug.WriteLine($"[MemoryService] OpenProcess handle: 0x{_processHandle.ToInt64():X}, Win32Error: {error}");

            if (_processHandle == IntPtr.Zero)
            {
                Debug.WriteLine($"[MemoryService] OpenProcess FAILED - Error: {error}");
                _isConnected = false;
                return false;
            }

            Debug.WriteLine("[MemoryService] Connected successfully!");
            _isConnected = true;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[MemoryService] Exception: {ex.Message}");
            _isConnected = false;
            return false;
        }
    }

    public bool TryReadUInt16(IntPtr address, out ushort value)
    {
        value = 0;
        if (!IsConnected || _processHandle == IntPtr.Zero) return false;

        try
        {
            var bytes = ReadBytes(_processHandle, address, 2);
            if (bytes == null) return false;
            value = (ushort)(bytes[0] + bytes[1] * 256);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[MemoryService] TryReadUInt16 Exception: {ex.Message}");
            return false;
        }
    }

    public bool TryReadInt32(IntPtr address, out int value)
    {
        value = 0;
        if (!IsConnected || _processHandle == IntPtr.Zero) return false;

        try
        {
            var bytes = ReadBytes(_processHandle, address, 4);
            if (bytes == null) return false;
            value = BitConverter.ToInt32(bytes, 0);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[MemoryService] TryReadInt32 Exception: {ex.Message}");
            return false;
        }
    }

    public bool TryReadBytes(IntPtr address, int size, out byte[] bytes)
    {
        bytes = Array.Empty<byte>();
        if (!IsConnected || _processHandle == IntPtr.Zero) return false;

        try
        {
            var result = ReadBytes(_processHandle, address, size);
            if (result == null) return false;
            bytes = result;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public IntPtr GetModuleBaseAddress()
    {
        if (_gameProcess == null || _gameProcess.HasExited)
        {
            return IntPtr.Zero;
        }

        try
        {
            return _gameProcess.MainModule?.BaseAddress ?? IntPtr.Zero;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[MemoryService] GetModuleBaseAddress Exception: {ex.Message}");
            return IntPtr.Zero;
        }
    }

    private byte[]? ReadBytes(IntPtr processHandle, IntPtr address, int size)
    {
        var buffer = new byte[size];
        if (ReadProcessMemory(processHandle, address, buffer, size, out var bytesRead))
        {
            if (bytesRead == size)
            {
                return buffer;
            }
            else
            {
                Debug.WriteLine($"[MemoryService] ReadBytes PARTIAL at 0x{address.ToInt64():X}, expected={size}, actual={bytesRead}");
                return null;
            }
        }

        var error = Marshal.GetLastWin32Error();
        Debug.WriteLine($"[MemoryService] ReadBytes FAILED at 0x{address.ToInt64():X}, Win32Error={error}");
        return null;
    }

    #region Windows API P/Invoke

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenProcess(
        uint dwDesiredAccess,
        bool bInheritHandle,
        int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        [Out] byte[] lpBuffer,
        int dwSize,
        out int lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    #endregion
}
