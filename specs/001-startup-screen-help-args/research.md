# Research: Startup Screen & Help Argument Listing

**Feature**: 001-startup-screen-help-args  
**Date**: 2026-04-08

## R-001: Current Welcome Screen Behavior

**Question**: What does the existing startup screen display and what needs to change?

**Findings**: The current `Program.Main()` already displays:
- Version string: `Welcome to Image Binarizer Application [Major.Minor.Build]`
- Copyright: `Copyright <c> daenet GmbH, All rights reserved.`
- When no arguments given: prints the ASCII art logo from `LogoPrinter.Print()` and exits

**Gaps identified**:
- The welcome screen does NOT explicitly display the brand name "ImageBinarizer" as a standalone title
- No help hint is displayed directing users to `-h` or `--help`
- The copyright uses `<c>` instead of the proper `©` symbol
- After printing the logo, the `PrintMessage(" ", ...)` call passes `isError=true`, which incorrectly shows the help hint format but with an empty error message

**Decision**: Enhance `Program.Main()` to display a clean branded header with the application name, version, copyright, ASCII art logo, and a prominent help usage hint.  
**Rationale**: Minimal changes to existing code structure; preserves backward compatibility.  
**Alternatives considered**: Using a third-party CLI framework (e.g., `System.CommandLine`, Spectre.Console) — rejected as overkill for this scope and would add unnecessary dependencies.

---

## R-002: Current Help Output Structure

**Question**: What does the existing help output display and what needs to change?

**Findings**: The `PrintHelp()` method in `CommandLineParsing.cs` currently displays:
- All 13 arguments with short/long forms
- Usage examples for common workflows
- Notes about boolean flags, default values, and ranges

**Gaps identified**:
- Help output is not structured in a table/columnar format — it uses inconsistent inline brace notation `{"-ip", "--input-image", ...}`
- No explicit indication of which arguments are **required** vs **optional**
- No explicit default value column (mentioned in prose but not per-argument)
- No value type indication (string, int, flag) per argument
- The `outputImagePath` is described as required, but it's actually only required when `--create-code` is NOT used

**Decision**: Restructure `PrintHelp()` to display arguments in a clean, aligned format with columns for: short form, long form(s), type, default, and description. Add a "Required/Optional" indicator. Keep existing usage examples.  
**Rationale**: Improves readability without changing the underlying argument parsing logic.  
**Alternatives considered**: Using `System.CommandLine` for auto-generated help — rejected to avoid adding a dependency for a simple feature.

---

## R-003: .NET 10 Target Framework Migration

**Question**: The user specified .NET 10 but the project currently targets `net8.0`. What is needed?

**Findings**: 
- All `.csproj` files currently target `net8.0`
- `Microsoft.Extensions.Configuration.CommandLine` 5.0.0 and `Microsoft.Extensions.Configuration.Binder` 5.0.0 are compatible with .NET 10
- `System.Drawing.Common` 8.0.7 — need to verify .NET 10 compatibility (may need update)
- MSTest packages are at older versions (2.2.3 / 16.9.4) — should work but may benefit from update

**Decision**: Update `TargetFramework` from `net8.0` to `net10.0` across all project files. Update NuGet package versions as needed for .NET 10 compatibility.  
**Rationale**: User explicitly requested .NET 10. This is a straightforward TFM change.  
**Alternatives considered**: Multi-targeting `net8.0;net10.0` — rejected as user did not request backward compatibility.

---

## R-004: Error Message Enhancement

**Question**: How are errors currently displayed and what needs to change?

**Findings**: The current error flow:
1. `CommandLineParsing.Parse()` returns `false` with an error message
2. `Program.Main()` calls `PrintMessage(errMsg, ConsoleColor.Red, true)`
3. `PrintMessage` with `isError=true` appends: `Insert one of these ["-h", "--help"] to following command for help: imgbin [command]`

**Gaps identified**:
- The help hint text is grammatically awkward ("Insert one of these ... to following command")
- When `errMsg` is null (e.g., help was invoked), the `isError=true` path still runs, printing "null" potentially
- The hint format could be cleaner: `Use "imgbin -h" or "imgbin --help" for usage information.`

**Decision**: Improve error message phrasing and ensure the help hint is always clear and grammatically correct.  
**Rationale**: Small text change with large UX improvement.  
**Alternatives considered**: None — this is a straightforward copy improvement.

---

## R-005: ASCII Art Logo Assessment

**Question**: Should the existing ASCII art logo be kept, modified, or replaced?

**Findings**: The current logo in `LogoPrinter.cs` is a 70×70 character binary art representation (using 0s and 1s) that appears to depict a stylized image/face. It is visually interesting and fits the "binarizer" theme.

**Decision**: Keep the existing ASCII art logo as-is. It aligns with the application's purpose (binary image representation) and is already distinctive.  
**Rationale**: The logo is on-brand and changing it is out of scope.  
**Alternatives considered**: Creating a new text-based "ImageBinarizer" banner — could be added above the binary art if desired, but not required by spec.
