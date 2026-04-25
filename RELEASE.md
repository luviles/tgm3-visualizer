## v1.0.1 — Optimization Update

**TGM3 Visualizer** is a real-time memory visualization tool for *Tetris: The Grand Master 3*.

### What's New in v1.0.1
- **Framework Upgrade:** Updated Windows App SDK to version 1.8 (1.8.260416003), bringing improved performance and framework stability.
- **Build Tools Update:** Updated `Microsoft.Windows.SDK.BuildTools` to version 10.0.26100.4654 for compatibility.
- **Cleanup:** Removed legacy debug file (`tgm3_debug.log`) creation logic to maintain a cleaner desktop environment for users.

---

## v1.0.0 — Initial Release

### Features

- **Real-time visualization** — automatically finds the game process and reads memory at 16 ms intervals
- **Non-intrusive** — read-only access; never modifies or injects anything into the game process
- **Account Info** — player nickname, decoration points, and personal best grades
- **4 Game Mode Dashboards**
  - **Easy Mode** — Level & Time, Hanabi Score, Section Times
  - **Master Mode** — Grade History, Demotion Progress, COOL/Regret indicators
  - **Sakura Mode** — Stage Timer & Limit, Jewel Block Tracking, Stage Progression
  - **Shirase Mode** — Grade System (S1–S13), Regret Deadline, Time Limits
- **Playfield** — real-time board visualization (works even during invisible staff rolls)
- **Lock Delay** — remaining lock delay indicator
- **Move Reset Counter** — tracks piece move resets before lock (World rule only)

### Download

| File | Platform | Size |
|------|----------|------|
| Tgm3Visualizer.exe | Windows x64 | 45,930,425 bytes (43.8 MB) |

**File integrity:**
```
SHA256: 43f5749373bf6b3a6cef22e2370550aeab0376a48fd0d2efce846bdd45152f43
```

> This is a **self-contained, single-file executable** built with .NET 8 and WinUI 3 (Windows App SDK 1.8). No separate runtime installation is required — just download and run.

### System Requirements

- Windows 10 version 1809 (build 17763) or later
- x64 architecture
- Installation of Windows App SDK 1.8 is required [#link](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/downloads)

### Technical Details

- **Framework:** .NET 8.0 (`net8.0-windows10.0.19041.0`)
- **UI Framework:** WinUI 3 (Windows App SDK 1.8.260416003)
- **Architecture:** x64 only
- **Deployment:** Self-contained single-file publish with compression enabled
- **MVVM Toolkit:** CommunityToolkit.Mvvm 8.2.2
- **Build Tools:** Microsoft.Windows.SDK.BuildTools 10.0.26100.4654
- **License:** MIT

### Disclaimer

This is an educational fan-made tool and is not affiliated with Arika Co., Ltd. Use at your own risk.