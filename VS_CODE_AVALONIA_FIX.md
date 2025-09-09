# VS Code Avalonia Fix Guide

## Problem:
- Avalonia extension can't build due to file locking
- Previewer fails with "dotnet build exited with code 1"

## Solutions:

### Solution 1: Kill Locked Processes
1. Open Task Manager (Ctrl+Shift+Esc)
2. Find "dotnet.exe" or "MunicipalApp.exe"
3. End task those processes
4. Run: `dotnet clean` then `dotnet build`

### Solution 2: Disable Auto-Build in Preview
1. Create `.vscode/settings.json` (already done)
2. Set `"avalonia.previewer.disableBuild": true`
3. Build manually first, then use preview

### Solution 3: Use Visual Studio Instead (Best)
1. Install Visual Studio 2022 Community (free)
2. Open your project
3. Double-click any .axaml file
4. Click "Design" tab at bottom
5. Use the built-in XAML designer

### Solution 4: Manual XAML Editing
1. Edit .axaml files directly in VS Code
2. Use IntelliSense for code completion
3. Run app to see changes: `dotnet run`

## For Now: Try This
1. Close VS Code
2. Open Task Manager and end all "dotnet.exe" processes
3. Reopen VS Code
4. Open terminal (Ctrl+`)
5. Run: `dotnet clean`
6. Run: `dotnet build`
7. Try the Avalonia preview again

## Quick Commands:
```cmd
# Clean build
dotnet clean

# Build project
dotnet build

# Run app
dotnet run

# Kill all dotnet processes
taskkill /F /IM dotnet.exe
```