<div align="center">

# STALZONE Region Switcher

Quick region switcher for the Steam client of **STALZONE**.  
Survives game updates.

**[Русский](README.md)** · English

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Windows](https://img.shields.io/badge/OS-Windows%2010%2F11-important)
![C#](https://img.shields.io/badge/C%23-5.0-239120?logo=csharp&logoColor=white)

</div>

## What it does

Creates / updates the `sc_forced_realm` file that controls the preferred region.

- `RU` → Russia
- `GLOBAL` → EU/NA/ASIA
- **Select automatically** → deletes the file; the game picks the region itself

After switching the region, just launch the game via the button in the app or through Steam as usual.

## How to use

1. Download the ready `STALZONERegionSwitcher.exe` from **[Releases](https://github.com/Helixleet/stalcraft-region-switcher/releases)**  
   **or** build it yourself (see below)
2. Run the program
3. It will try to find the game folder by AppID `1818450`  
   • If not found → click “Change” and select the game folder manually
4. Click “Change region” / “Select region” → choose Russia, EU/NA/ASIA, or “Select automatically”
5. Click “Launch game” (or start the game yourself via Steam)

## Building from source

### Easiest way (no Visual Studio)

Only Windows 10/11 with .NET Framework 4.x is required (already installed).

```bat
git clone https://github.com/Helixleet/stalcraft-region-switcher.git
cd stalcraft-region-switcher
build.bat
```

Output: `bin\STALZONERegionSwitcher.exe`

### Via Visual Studio

1. Open `STALZONERegionSwitcher.csproj` in Visual Studio 2019/2022
2. Configuration: **Release | Any CPU**
3. Build → Build Solution

> **Do not use** `dotnet build` — this project targets .NET Framework 4.8, not modern .NET.

## How it works

- Locates Steam via the registry
- Reads `libraryfolders.vdf` and `appmanifest_1818450.acf`
- Finds the game folder and validates `steam_appid.txt`
- Launches the game with: `steam://rungameid/1818450`

## History

Originally written in Python (PyQt6). Rewritten in C# / .NET Framework 4.8 so no extra runtime or dependencies are needed.

## License

[MIT](LICENSE)
