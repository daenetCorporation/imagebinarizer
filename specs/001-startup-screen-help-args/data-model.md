# Data Model: Startup Screen & Help Argument Listing

**Feature**: 001-startup-screen-help-args  
**Date**: 2026-04-08

## Entities

### ArgumentDefinition

Represents a single command-line argument recognized by the application.

| Field | Type | Description |
|-------|------|-------------|
| ShortForm | string | Short flag (e.g., `-ip`) |
| LongForms | string[] | Long flag(s) (e.g., `--input-image`, `--inputImagePath`) |
| Description | string | Human-readable description of the argument's purpose |
| ValueType | enum | One of: `String`, `Integer`, `Flag` (boolean) |
| IsRequired | bool | Whether the argument must be provided |
| DefaultValue | string | Display string for the default (e.g., `"auto"`, `"0"`, `"false"`) |
| ValidRange | string? | Optional valid range description (e.g., `"0-255"`) |

### ArgumentCatalog (all 13 arguments)

| # | ShortForm | LongForm(s) | ValueType | Required | Default | ValidRange | Description |
|---|-----------|-------------|-----------|----------|---------|------------|-------------|
| 1 | `-ip` | `--input-image` | String | Yes | — | — | Path to the input image file |
| 2 | `-op` | `--output-image` | String | No* | — | — | Path for the binarized output file |
| 3 | `-iw` | `--imageWidth` | Integer | No | `0` (original) | ≥ 0 | Custom width for the binarized output |
| 4 | `-ih` | `--imageHeight` | Integer | No | `0` (original) | ≥ 0 | Custom height for the binarized output |
| 5 | `-rt` | `--redThreshold` | Integer | No | `-1` (auto) | 0–255 | Red channel threshold for binarization |
| 6 | `-gt` | `--greenThreshold` | Integer | No | `-1` (auto) | 0–255 | Green channel threshold for binarization |
| 7 | `-bt` | `--blueThreshold` | Integer | No | `-1` (auto) | 0–255 | Blue channel threshold for binarization |
| 8 | `-grt` | `--greyThreshold` | Integer | No | `-1` (auto) | 0–255 | Grey scale threshold (use with `-gs`) |
| 9 | `-inv` | `--inverse` | Flag | No | `false` | — | Inverse the contrast of the binarized image |
| 10 | `-gs` | `--greyscale` | Flag | No | `false` | — | Use grey scale threshold mode |
| 11 | `-cc` | `--createcode`, `--create-code` | Flag | No | `false` | — | Generate a .cs code file for logo printing |
| 12 | `-gc` | `--getcontour` | Flag | No | `false` | — | Extract contour from the image |
| 13 | `-h` | `--help` | Flag | No | `false` | — | Display help and argument listing |

*`-op` is required for binarization but auto-defaults to `.\LogoPrinter.cs` when `--create-code` is used.

### WelcomeScreen

Displayed when the application launches with no arguments.

| Field | Source | Description |
|-------|--------|-------------|
| ApplicationName | Hardcoded | `"ImageBinarizer"` |
| Version | `Assembly.GetExecutingAssembly().GetName().Version` | `Major.Minor.Build` format |
| Copyright | Hardcoded | `"Copyright © daenet GmbH, All rights reserved."` |
| Logo | `LogoPrinter.Print()` | ASCII art binary representation |
| HelpHint | Hardcoded | `"Use 'imgbin -h' or 'imgbin --help' for usage information."` |

### HelpScreen

Displayed when `-h` or `--help` is provided.

| Field | Description |
|-------|-------------|
| Header | Application name + version (same as welcome screen header) |
| ArgumentTable | Formatted listing of all 13 arguments from ArgumentCatalog |
| UsageExamples | Curated list of common command patterns |
| Notes | Additional guidance on boolean flags, default values, ranges |

## Relationships

```
BinarizerParams (base class, 13 properties)
  └── BinarizerConfiguration (adds Help flag)
        └── CommandLineParsing (maps CLI args → BinarizerConfiguration)
              └── Program.Main (dispatches to WelcomeScreen or HelpScreen or Binarization)
```

## State Transitions

```
Application Launch
  ├── No arguments → WelcomeScreen (logo + help hint) → Exit
  ├── -h / --help → HelpScreen (argument listing + examples) → Exit
  ├── Valid arguments → Binarization → Success message → Exit
  └── Invalid arguments → Error message + help hint → Exit
```

## Validation Rules

- `InputImagePath`: Must be a valid file path to an existing file
- `OutputImagePath`: Parent directory must exist; auto-set to `.\LogoPrinter.cs` when `--create-code` used
- `ImageWidth`, `ImageHeight`: Must be ≥ 0 (0 = use original dimensions)
- `RedThreshold`, `GreenThreshold`, `BlueThreshold`, `GreyThreshold`: Must be -1 (auto) or 0–255
- Boolean flags (`-inv`, `-gs`, `-cc`, `-gc`, `-h`): Presence sets to `true`; absence defaults to `false`
