# CLI Contract: ImageBinarizer Tool (`imgbin`)

**Feature**: 001-startup-screen-help-args  
**Date**: 2026-04-08  
**Type**: Command-line interface contract

## Command Name

```
imgbin
```

Installed as a .NET global tool via `dotnet tool install`.

## Invocation Patterns

### 1. No Arguments → Welcome Screen

```
imgbin
```

**Output** (stdout):
```
Welcome to ImageBinarizer [1.5.7]
Copyright © daenet GmbH, All rights reserved.

[ASCII art logo]

Use "imgbin -h" or "imgbin --help" for usage information.
```

**Exit code**: 0

---

### 2. Help Argument → Argument Listing

```
imgbin -h
imgbin --help
```

**Output** (stdout):
```
ImageBinarizer [1.5.7] - .NET Global Tool for image binarization.
Copyright © daenet GmbH, All rights reserved.

Usage: imgbin [options]

Options:
  -ip,  --input-image       Input image file path (required)          [string]
  -op,  --output-image      Output file path                          [string]
  -iw,  --imageWidth        Custom output width (0 = original)        [int, default: 0]
  -ih,  --imageHeight       Custom output height (0 = original)       [int, default: 0]
  -rt,  --redThreshold      Red channel threshold (-1 = auto)         [int, 0-255, default: -1]
  -gt,  --greenThreshold    Green channel threshold (-1 = auto)       [int, 0-255, default: -1]
  -bt,  --blueThreshold     Blue channel threshold (-1 = auto)        [int, 0-255, default: -1]
  -grt, --greyThreshold     Grey scale threshold (-1 = auto)          [int, 0-255, default: -1]
  -inv, --inverse           Inverse binarized image contrast           [flag]
  -gs,  --greyscale         Use grey scale threshold mode              [flag]
  -cc,  --create-code       Generate .cs code file for logo printing   [flag]
  -gc,  --getcontour        Extract contour from image                 [flag]
  -h,   --help              Show this help message                     [flag]

Examples:
  imgbin --input-image c:\a.png --output-image d:\out.txt
  imgbin --input-image c:\a.png --output-image d:\out.txt -iw 32 -ih 32
  imgbin --input-image c:\a.png --output-image d:\out.txt -rt 100 -gt 100 -bt 100
  imgbin --input-image c:\a.png --output-image d:\out.txt -inv
  imgbin --input-image c:\a.png --output-image d:\out.txt -iw 32 -ih 32 -grt 100 -gs
  imgbin --input-image c:\a.png --output-image d:\out.txt -inv -gc
  imgbin --input-image c:\a.png --create-code
  imgbin --input-image c:\a.png -iw 150 --create-code
```

**Exit code**: 0

---

### 3. Valid Arguments → Binarization

```
imgbin --input-image <path> --output-image <path> [options]
```

**Output** (stdout): Progress and completion messages.  
**Exit code**: 0

---

### 4. Invalid Arguments → Error + Help Hint

```
imgbin --input-image nonexistent.png --output-image out.txt
```

**Output** (stdout):
```
Welcome to ImageBinarizer [1.5.7]
Copyright © daenet GmbH, All rights reserved.

Error: Input file doesn't exist.

Use "imgbin -h" or "imgbin --help" for usage information.
```

**Exit code**: 0 (current behavior — no exit code differentiation)

## Argument Specification

| Argument | Short | Long | Required | Type | Default | Valid Range |
|----------|-------|------|----------|------|---------|-------------|
| Input image | `-ip` | `--input-image` | Yes | string | — | Valid file path |
| Output image | `-op` | `--output-image` | No* | string | — | Valid directory |
| Width | `-iw` | `--imageWidth` | No | int | 0 | ≥ 0 |
| Height | `-ih` | `--imageHeight` | No | int | 0 | ≥ 0 |
| Red threshold | `-rt` | `--redThreshold` | No | int | -1 | -1 to 255 |
| Green threshold | `-gt` | `--greenThreshold` | No | int | -1 | -1 to 255 |
| Blue threshold | `-bt` | `--blueThreshold` | No | int | -1 | -1 to 255 |
| Grey threshold | `-grt` | `--greyThreshold` | No | int | -1 | -1 to 255 |
| Inverse | `-inv` | `--inverse` | No | flag | false | — |
| Grey scale | `-gs` | `--greyscale` | No | flag | false | — |
| Create code | `-cc` | `--create-code` | No | flag | false | — |
| Get contour | `-gc` | `--getcontour` | No | flag | false | — |
| Help | `-h` | `--help` | No | flag | false | — |

*Output image defaults to `.\LogoPrinter.cs` when `--create-code` is used.
