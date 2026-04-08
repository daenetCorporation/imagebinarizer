# Implementation Plan: Startup Screen & Help Argument Listing

**Branch**: `001-startup-screen-help-args` | **Date**: 2026-04-08 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/001-startup-screen-help-args/spec.md`

## Summary

Improve the ImageBinarizer CLI tool's startup experience and help output. When launched without arguments, display a branded welcome screen with the application name, version, copyright, ASCII art logo, and a help hint. When launched with `-h`/`--help`, display a structured, complete listing of all 13 command-line arguments with short/long forms, descriptions, value types, ranges, defaults, and usage examples. Enhance error messages to include help hints. The existing codebase already has partial implementations of both the welcome screen and help output — this feature upgrades them to be complete and well-structured.

## Technical Context

**Language/Version**: C# / .NET 10  
**Primary Dependencies**: Microsoft.Extensions.Configuration.CommandLine, Microsoft.Extensions.Configuration.Binder  
**Storage**: N/A (file I/O only for image processing, not for this feature)  
**Testing**: MSTest (Microsoft.VisualStudio.TestTools.UnitTesting)  
**Target Platform**: Cross-platform CLI (.NET global tool, command name `imgbin`)  
**Project Type**: CLI tool (packaged as .NET global tool) + library (NuGet package)  
**Performance Goals**: Help/welcome screen renders instantly (<100ms)  
**Constraints**: No additional NuGet dependencies; console output only (stdout/stderr)  
**Scale/Scope**: Single CLI application, 13 arguments, ~6 source files affected

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Constitution has no custom principles defined (template placeholders only). No gates to evaluate. Proceeding.

## Project Structure

### Documentation (this feature)

```text
specs/001-startup-screen-help-args/
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output
└── tasks.md             # Phase 2 output (NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
source/
├── ImageBinarizer/              # Core library (NuGet package)
│   ├── ImageBinarizer.cs        # Core binarization logic
│   ├── CodeCreator.cs           # .cs file code generation
│   ├── ImagePixelsDataHandler.cs
│   ├── Entities/
│   │   └── BinarizerParams.cs   # Base parameter entity (13 properties)
│   └── ExtensionMethod/
│       ├── BitmapExtension.cs
│       └── ImageBinarizerExtension.cs
├── ImageBinarizerTool/          # CLI tool (global tool)
│   ├── Program.cs               # Entry point — welcome screen + dispatch
│   ├── CommandLineParsing.cs     # Argument parsing + help output
│   ├── LogoPrinter.cs           # ASCII art logo
│   └── Entities/
│       └── BinarizerConfiguration.cs  # Extends BinarizerParams with Help flag
├── ImageBinarizerUnitTest/      # Unit tests (MSTest)
│   └── ImageBinarizerUnitTest.cs
├── Sample/                      # Sample usage project
└── VideoBinarizerTool/          # Video binarization tool
```

**Structure Decision**: Existing multi-project structure is preserved. Changes are scoped to `ImageBinarizerTool` (CLI) and `ImageBinarizerUnitTest` (tests). No new projects needed.

## Complexity Tracking

No constitution violations. No complexity justifications needed.
