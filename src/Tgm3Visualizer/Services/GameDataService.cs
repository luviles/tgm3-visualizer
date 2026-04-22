using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Dispatching;
using CommunityToolkit.Mvvm.ComponentModel;
using Tgm3Visualizer.Core.Interfaces;
using Tgm3Visualizer.Core.Models;
using Tgm3Visualizer.Core.Processors;

namespace Tgm3Visualizer.Services;

public partial class GameDataService : ObservableObject
{
    private readonly IMemoryReader _memoryReader;
    private readonly MasterModeProcessor _masterProcessor = new();
    private readonly ShiraseModeProcessor _shiraseProcessor = new();
    private readonly EasyModeProcessor _easyProcessor = new();
    private readonly SakuraModeProcessor _sakuraProcessor = new();
    private readonly DispatcherQueueTimer _timer;

    [ObservableProperty] private ProcessedGameState _currentState = new();
    [ObservableProperty] private bool _isGameRunning;

    public GameDataService(IMemoryReader memoryReader)
    {
        _memoryReader = memoryReader;

        _timer = DispatcherQueue.GetForCurrentThread().CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(8);
        _timer.Tick += OnTimerTick;
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    private void OnTimerTick(DispatcherQueueTimer sender, object args)
    {
        if (!_memoryReader.IsConnected)
        {
            _memoryReader.TryConnect();
            IsGameRunning = _memoryReader.IsConnected;
            return;
        }

        var rawState = ReadGameState();
        if (rawState == null)
        {
            return;
        }

        CurrentState = rawState.GameType switch
        {
            GameType.Master => _masterProcessor.Process(rawState),
            GameType.Qualified_Master => _masterProcessor.Process(rawState),
            GameType.Shirase => _shiraseProcessor.Process(rawState),
            GameType.Easy => _easyProcessor.Process(rawState),
            GameType.Sakura => _sakuraProcessor.Process(rawState),
            _ => CreateCommonState(rawState)
        };
    }

    private GameState? ReadGameState()
    {
        if (!_memoryReader.TryReadUInt16((IntPtr)MemoryAddresses.Level, out var level))
        {
            return null;
        }
        if (!_memoryReader.TryReadInt32((IntPtr)MemoryAddresses.Time, out var time))
        {
            return null;
        }
        if (!_memoryReader.TryReadInt32((IntPtr)MemoryAddresses.GameType, out var gameType))
        {
            return null;
        }

        // Read Final Grade (absolute address)
        int finalGrade = 0;
        if (_memoryReader.TryReadUInt16((IntPtr)MemoryAddresses.FinalGrade, out var gradeValue))
        {
            finalGrade = gradeValue;
        }

        // Read Awarded Grade Points (1 byte)
        int awardedGradePoints = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.AwardedGradePoints, 1, out var pointsBytes))
        {
            awardedGradePoints = pointsBytes[0];
        }

        // Read Decay Time (1 byte)
        int decayTime = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.DecayTime, 1, out var decayBytes))
        {
            decayTime = decayBytes[0];
        }

        // Read Section Grade Points (1 byte)
        int sectionGradePoints = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.SectionGradePoints, 1, out var sgpBytes))
        {
            sectionGradePoints = sgpBytes[0];
        }

        // Read Section Regret Count (1 byte)
        int sectionRegretCount = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.SectionRegretCount, 1, out var srcBytes))
        {
            sectionRegretCount = srcBytes[0];
        }

        // Read Internal Grade (1 byte)
        int internalGrade = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.InternalGrade, 1, out var igd1Bytes))
        {
            internalGrade = igd1Bytes[0];
        }

        // Read Staff Roll Time (2 bytes)
        int staffRollTime = 0;
        if (_memoryReader.TryReadUInt16((IntPtr)MemoryAddresses.StaffRollTime, out var srtValue))
        {
            staffRollTime = srtValue;
        }

        // Read Staff Roll Tetris Count (2 bytes)
        int staffRollTetrisCount = 0;
        if (_memoryReader.TryReadUInt16((IntPtr)MemoryAddresses.StaffRollTetrisCount, out var srtcValue))
        {
            staffRollTetrisCount = srtcValue;
        }

        // Read Staff Roll Line Clear Count (2 bytes)
        int staffRollLineClear = 0;
        if (_memoryReader.TryReadUInt16((IntPtr)MemoryAddresses.StaffRollLineClear, out var srlcValue))
        {
            staffRollLineClear = srlcValue;
        }

        // Read Game Process Status (1 byte)
        int gameProcessStatus = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.GameProcessStatus, 1, out var gpsBytes))
        {
            gameProcessStatus = gpsBytes[0];
        }

        // Read Field Status (1 byte)
        int fieldStatus = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.FieldStatus, 1, out var fsBytes))
        {
            fieldStatus = fsBytes[0];
        }

        // Read Game Flag (1 byte)
        int gameFlag = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.GameFlag, 1, out var gfBytes))
        {
            gameFlag = gfBytes[0];
        }

        #region Player Info

        // Read Nickname (3 bytes ASCII)
        string nickname = "";
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.Nickname1, 3, out var nickBytes))
        {
            nickname = new string(nickBytes.Where(b => b >= 32).Select(b => (char)b).ToArray());
        }
        
        // Read Playtime (4 bytes)
        int playtime = 0;
        if (_memoryReader.TryReadInt32((IntPtr)MemoryAddresses.Playtime, out var ptValue))
        {
            playtime = Math.Min(ptValue, 99999);
        }

        // Read Decoration Points (4 bytes)
        int decorationPoints = 0;
        if (_memoryReader.TryReadInt32((IntPtr)MemoryAddresses.DecorationPoints, out var dpValue))
        {
            decorationPoints = Math.Min(dpValue, 100000);
        }

        // Read World Easy Hi-Score (2 bytes)
        int worldEasyHiScore = 0;
        if (_memoryReader.TryReadUInt16((IntPtr)MemoryAddresses.WorldEasyHiScore, out var worldEasyScore))
        {
            worldEasyHiScore = worldEasyScore;
        }

        // Read Classic Easy Hi-Score (2 bytes)
        int classicEasyHiScore = 0;
        if (_memoryReader.TryReadUInt16((IntPtr)MemoryAddresses.ClassicEasyHiScore, out var classicEasyScore))
        {
            classicEasyHiScore = classicEasyScore;
        }

        // Read World Sakura Hi-Grade (1 byte)
        int worldSakuraHiGrade = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.WorldSakuraHiGrade, 1, out var worldSakuraBytes))
        {
            worldSakuraHiGrade = worldSakuraBytes[0];
        }

        // Read Classic Sakura Hi-Grade (1 byte)
        int classicSakuraHiGrade = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.ClassicSakuraHiGrade, 1, out var classicSakuraBytes))
        {
            classicSakuraHiGrade = classicSakuraBytes[0];
        }

        // Read World Master Current Grade (1 byte)
        int worldMasterCurrentGrade = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.WorldMasterCurrentGrade, 1, out var worldMcgBytes))
        {
            worldMasterCurrentGrade = worldMcgBytes[0];
        }

        // Read Classic Master Current Grade (1 byte)
        int classicMasterCurrentGrade = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.ClassicMasterCurrentGrade, 1, out var classicMcgBytes))
        {
            classicMasterCurrentGrade = classicMcgBytes[0];
        }

        // Read World Shirase Hi-Grade (1 byte)
        int worldShiraseHiGrade = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.WorldShiraseHiGrade, 1, out var worldShgBytes))
        {
            worldShiraseHiGrade = worldShgBytes[0];
        }

        // Read Classic Shirase Hi-Grade (1 byte)
        int classicShiraseHiGrade = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.ClassicShiraseHiGrade, 1, out var classicShgBytes))
        {
            classicShiraseHiGrade = classicShgBytes[0];
        }

        // Read World Master Grade History (7 bytes, most recent at higher address)
        int[] worldMasterGradeHistory = new int[7];
        for (int i = 0; i < 7; i++)
        {
            if (_memoryReader.TryReadBytes((IntPtr)(MemoryAddresses.WorldMasterGradeHistoryStart + i), 1, out var worldHistBytes))
            {
                worldMasterGradeHistory[6 - i] = worldHistBytes[0]; // Reverse: oldest first
            }
        }

        // Read Classic Master Grade History (7 bytes, most recent at higher address)
        int[] classicMasterGradeHistory = new int[7];
        for (int i = 0; i < 7; i++)
        {
            if (_memoryReader.TryReadBytes((IntPtr)(MemoryAddresses.ClassicMasterGradeHistoryStart + i), 1, out var classicHistBytes))
            {
                classicMasterGradeHistory[6 - i] = classicHistBytes[0]; // Reverse: oldest first
            }
        }

        // Read World Demotion Points (1 byte)
        int worldDemotionPoints = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.WorldDemotionPoints, 1, out var worldDpBytes))
        {
            worldDemotionPoints = worldDpBytes[0];
        }

        // Read Classic Demotion Points (1 byte)
        int classicDemotionPoints = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.ClassicDemotionPoints, 1, out var classicDpBytes))
        {
            classicDemotionPoints = classicDpBytes[0];
        }

        #endregion

        #region Sakura Mode

        // Read Sakura Stage Limit Time (4 bytes)
        int sakuraStageLimitTime = 0;
        if (_memoryReader.TryReadInt32((IntPtr)MemoryAddresses.SakuraStageLimitTime, out var ssltValue))
        {
            sakuraStageLimitTime = ssltValue;
        }

        // Read Sakura Stage Elapsed Time (4 bytes)
        int sakuraStageElapsedTime = 0;
        if (_memoryReader.TryReadInt32((IntPtr)MemoryAddresses.SakuraStageElapsedTime, out var ssetValue))
        {
            sakuraStageElapsedTime = ssetValue;
        }

        // Read Sakura Stage Level (1 byte)
        int sakuraStageLevel = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.SakuraStageLevel, 1, out var sslBytes))
        {
            sakuraStageLevel = sslBytes[0];
        }

        // Read Sakura Cleared Stage Level (1 byte)
        int sakuraClearedStageLevel = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.SakuraClearedStageLevel, 1, out var scslBytes))
        {
            sakuraClearedStageLevel = scslBytes[0];
        }

        #endregion

        #region Master Mode Exam

        // Read Master Exam Flags (1 byte from 0x4AE32B)
        int masterExamFlags = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.MasterExamFlags, 1, out var mefBytes))
        {
            masterExamFlags = mefBytes[0];
        }

        // Read 70% checkpoint counter (1 byte from 0x4ACD8C)
        int seventyPercentCheckpointCount = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.SeventyPercentCheckpointCount, 1, out var spccBytes))
        {
            seventyPercentCheckpointCount = spccBytes[0];
        }

        // Read Section COOL flag (1 byte from 0x4AE306)
        int sectionCoolFlag = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.SectionCoolFlag, 1, out var scfBytes))
        {
            sectionCoolFlag = scfBytes[0];
        }

        #endregion

        #region Playfield

        // Read Playfield (10x22)
        // Each cell is 8 bytes apart, each row is 96 bytes (12 cells × 8 bytes)
        byte[,] playfield = new byte[22, 10];
        for (int y = 0; y < 22; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                var addr = MemoryAddresses.Playfield1P + (y * 96) + (x * 8);
                if (_memoryReader.TryReadBytes((IntPtr)addr, 2, out var cellBytes))
                {
                    byte blockType = cellBytes[0];
                    if (cellBytes[1] == 1)
                    {
                        if (blockType >= 0x02 && blockType <= 0x08)
                            blockType = 0x09;
                        else if (blockType >= 0x12 && blockType <= 0x18)
                            blockType = 0x19;
                    }
                    playfield[y, x] = blockType;
                }
            }
        }

        #endregion

        #region Animation / Lock Delay

        // Read Animation Delay (2 bytes from 0x4AE244)
        int animationDelay = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.AnimationDelay, 2, out var adBytes))
        {
            animationDelay = BitConverter.ToInt16(adBytes, 0);
        }

        // Read Lock Delay (1 byte from 0x4AF301)
        int lockDelay = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.LockDelay, 1, out var ldBytes))
        {
            lockDelay = ldBytes[0];
        }

        // Read Shift Move Reset Count (1 byte from 0x4AF2EA)
        int shiftMoveResetCount = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.ShiftMoveResetCount, 1, out var smrcBytes))
        {
            shiftMoveResetCount = smrcBytes[0];
        }

        // Read Rotation Move Reset Count (1 byte from 0x4AF2E8)
        int rotationMoveResetCount = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.RotationMoveResetCount, 1, out var rmrcBytes))
        {
            rotationMoveResetCount = rmrcBytes[0];
        }

        // Read Garbage Quota (1 byte from 0x4AF2EF)
        int garbageQuota = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.GarbageQuota, 1, out var gqBytes))
        {
            garbageQuota = gqBytes[0];
        }

        // Read Hanabi Score (2 bytes from 0x4AE358, Easy mode)
        int hanabiScore = 0;
        if (_memoryReader.TryReadBytes((IntPtr)MemoryAddresses.HanabiScore, 2, out var hsBytes))
        {
            hanabiScore = BitConverter.ToUInt16(hsBytes, 0);
        }

        #endregion

        return new GameState
        {
            Level = level,
            TimeFrames = time,
            GameType = (GameType)gameType,
            FinalGrade = finalGrade,
            SectionGradePoints = sectionGradePoints,
            SectionRegretCount = sectionRegretCount,
            AwardedGradePoints = awardedGradePoints,
            DecayTime = decayTime,
            InternalGrade = internalGrade,
            StaffRollTime = staffRollTime,
            StaffRollTetrisCount = staffRollTetrisCount,
            StaffRollLineClear = staffRollLineClear,
            Nickname = nickname,
            Playtime = playtime,
            DecorationPoints = decorationPoints,
            WorldEasyHiScore = worldEasyHiScore,
            ClassicEasyHiScore = classicEasyHiScore,
            WorldSakuraHiGrade = worldSakuraHiGrade,
            ClassicSakuraHiGrade = classicSakuraHiGrade,
            WorldMasterCurrentGrade = worldMasterCurrentGrade,
            ClassicMasterCurrentGrade = classicMasterCurrentGrade,
            WorldShiraseHiGrade = worldShiraseHiGrade,
            ClassicShiraseHiGrade = classicShiraseHiGrade,
            WorldMasterGradeHistory = worldMasterGradeHistory,
            ClassicMasterGradeHistory = classicMasterGradeHistory,
            WorldDemotionPoints = worldDemotionPoints,
            ClassicDemotionPoints = classicDemotionPoints,
            GameProcessStatus = gameProcessStatus,
            FieldStatus = fieldStatus,
            GameFlag = gameFlag,
            Playfield = playfield,
            AnimationDelay = animationDelay,
            LockDelay = lockDelay,
            ShiftMoveResetCount = shiftMoveResetCount,
            RotationMoveResetCount = rotationMoveResetCount,
            GarbageQuota = garbageQuota,
            HanabiScore = hanabiScore,
            SakuraStageLimitTime = sakuraStageLimitTime,
            SakuraStageElapsedTime = sakuraStageElapsedTime,
            SakuraStageLevel = sakuraStageLevel,
            SakuraClearedStageLevel = sakuraClearedStageLevel,
            MasterExamFlags = masterExamFlags,
            SeventyPercentCheckpointCount = seventyPercentCheckpointCount,
            SectionCoolFlag = sectionCoolFlag
        };
    }

    private static ProcessedGameState CreateCommonState(GameState rawState)
    {
        var result = new ProcessedGameState();
        GameStateProcessorBase.CopyCommonFields(rawState, result);
        return result;
    }
}
