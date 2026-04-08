# Quickstart: Startup Screen & Help Argument Listing

**Feature**: 001-startup-screen-help-args  
**Date**: 2026-04-08

## Overview

This feature enhances the ImageBinarizer CLI tool (`imgbin`) with an improved startup welcome screen and a structured help argument listing. The changes are scoped to the `ImageBinarizerTool` project and its unit tests.

## What Changes

### Files to Modify

1. **`source/ImageBinarizerTool/Program.cs`** — Enhance welcome screen display: add branded header, fix copyright symbol, add help hint after logo
2. **`source/ImageBinarizerTool/CommandLineParsing.cs`** — Restructure `PrintHelp()` to show a formatted argument table with types, defaults, and required/optional indicators
3. **`source/ImageBinarizerTool/ImageBinarizerTool.csproj`** — Update `TargetFramework` to `net10.0`
4. **`source/ImageBinarizer/ImageBinarizer.csproj`** — Update `TargetFramework` to `net10.0`
5. **`source/ImageBinarizerUnitTest/ImageBinarizerUnitTest.csproj`** — Update `TargetFramework` to `net10.0`
6. **`source/Sample/Sample.csproj`** — Update `TargetFramework` to `net10.0`
7. **`source/VideoBinarizerTool/VideoBinarizer.csproj`** — Update `TargetFramework` to `net10.0`

### Files to Add

8. **`source/ImageBinarizerUnitTest/CommandLineParsingTests.cs`** — Unit tests for help output content and welcome screen behavior

### No Files Deleted

## Key Design Decisions

- **No new dependencies**: Restructured help output uses only `Console.WriteLine` with manual alignment. No third-party CLI framework needed.
- **Preserve existing behavior**: Argument parsing logic in `CommandLineParsing` and `GetCommandLineMap()` remains unchanged. Only the display formatting is updated.
- **ASCII art logo kept**: The existing binary art in `LogoPrinter.cs` is preserved as-is.
- **.NET 10 upgrade**: All projects updated from `net8.0` to `net10.0` per user request.

## Build & Run

```bash
# Build
dotnet build source/ImageBinarizer.sln

# Run (no args — welcome screen)
dotnet run --project source/ImageBinarizerTool

# Run (help)
dotnet run --project source/ImageBinarizerTool -- -h

# Test
dotnet test source/ImageBinarizerUnitTest
```

## Verification

After implementation, verify:
1. `imgbin` with no args shows: branded name, version, copyright ©, ASCII logo, help hint
2. `imgbin -h` shows: all 13 arguments with short/long forms, types, defaults, ranges, examples
3. `imgbin --help` produces identical output to `imgbin -h`
4. Invalid args show: error message + help hint
5. All existing unit tests pass
6. New unit tests for help content pass
