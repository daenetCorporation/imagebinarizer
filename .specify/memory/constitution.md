<!--
Sync Impact Report
===================
Version change: N/A → 1.0.0 (initial creation)
Modified principles: N/A (all new)
Added sections:
  - Core Principles (6 principles)
  - Technology Standards
  - Development Workflow
  - Governance
Removed sections: None
Templates requiring updates:
  - .specify/templates/plan-template.md ✅ no update needed (dynamic gate)
  - .specify/templates/spec-template.md ✅ no update needed
  - .specify/templates/tasks-template.md ✅ no update needed
Follow-up TODOs: None
-->

# ImageBinarizer Constitution

## Core Principles

### I. Library-First Architecture

All image-processing functionality MUST reside in the core
`ImageBinarizer` library project. The library MUST be:

- Self-contained and independently consumable as a NuGet package.
- Free of CLI or host-specific concerns (no console I/O, no
  `System.Environment` calls).
- Usable both standalone (`Run()`, `GetStringBinary()`,
  `GetArrayBinary()`) and as a pipeline module
  (`IPipelineModule<double[,,], double[,,]>`).
- The single source of truth for binarization, grayscale,
  contour extraction, and code-generation logic.

Consumer projects (CLI tool, video tool, samples) MUST depend
on the library — never duplicate its logic.

### II. CLI Interface Contract

The `ImageBinarizerTool` (`imgbin`) is the primary user-facing
entry point and MUST adhere to these rules:

- Every argument MUST have a short form (`-ip`) and, where
  applicable, a long form (`--input-image`).
- Boolean flags MUST default to `false` and work as toggles.
- When launched with no arguments: display branded welcome screen
  (name, version, copyright, ASCII logo) and a help hint.
- When launched with `-h` / `--help`: display the complete
  argument listing with types, defaults, and usage examples.
- Invalid input MUST produce a clear error message on `stderr`
  with a hint to use `--help`.
- Exit code 0 on success; non-zero on any failure.

### III. Configuration-Driven Design

All processing behavior MUST be controlled through the
`BinarizerParams` hierarchy:

- `BinarizerParams` is the base configuration model shared by
  library and tool. It MUST NOT contain host-specific properties.
- `BinarizerConfiguration` extends `BinarizerParams` with
  tool-only properties (e.g., `Help`).
- New processing options MUST be added as properties on the
  appropriate configuration class with sensible defaults.
- Configuration binding from command-line arguments MUST use
  `Microsoft.Extensions.Configuration.CommandLine` and
  `Microsoft.Extensions.Configuration.Binder`.

### IV. Testing Discipline

Every feature that changes observable behavior MUST have
corresponding unit tests:

- Testing framework: MSTest (`Microsoft.VisualStudio.TestTools.UnitTesting`).
- Tests MUST cover: output dimensions, threshold boundaries per
  channel (R, G, B, grey), default vs. custom configuration.
- Test data MUST be deterministic — use programmatically generated
  bitmaps for threshold tests; use repository-local images
  (`CommonFiles/`) for integration-level tests.
- Tests MUST NOT depend on network resources or external services.
- Code coverage collection via Coverlet MUST remain enabled.

### V. Cross-Platform Compatibility

The solution targets .NET 10 and MUST remain cross-platform:

- Image decoding MUST use SkiaSharp (`SKBitmap.Decode`) as the
  primary path. `System.Drawing.Common` usage MUST be limited to
  extension utilities that do not break on non-Windows hosts.
- Platform-specific native assets (e.g.,
  `SkiaSharp.NativeAssets.Linux`) MUST be declared as NuGet
  dependencies, not bundled binaries.
- File paths in code and tests MUST use `Path.Combine` or
  relative paths — never hard-coded Windows separators.

### VI. Simplicity & YAGNI

- Start with the simplest implementation that satisfies the
  specification. Do not add features, abstractions, or
  configuration options speculatively.
- No new project SHOULD be added to the solution unless it serves
  a distinct packaging or deployment target.
- Helper classes and extension methods MUST justify their
  existence by being used in more than one call site or by
  encapsulating unsafe/interop code (`Marshal.Copy`, `LockBits`).

## Technology Standards

- **Runtime**: .NET 10.0 (`net10.0`) for all projects.
- **Language**: C# with standard .NET conventions — PascalCase
  for public members, camelCase for parameters and locals.
- **Namespaces**: `Daenet.Binarizer` (library),
  `Daenet.Binarizer.Entities`, `Daenet.Binarizer.ExtensionMethod`,
  `Daenet.ImageBinarizerTool` (CLI tool).
- **Packaging**: `ImageBinarizer` as a NuGet library package;
  `ImageBinarizerTool` as a .NET global tool (`PackAsTool=true`,
  command `imgbin`). Both MUST auto-generate packages on build
  (`GeneratePackageOnBuild=True`).
- **Versioning**: `MAJOR.MINOR.PATCH` in `.csproj` `<Version>`.
  Bump MAJOR for breaking API/CLI changes, MINOR for new features,
  PATCH for bug fixes.
- **Dependencies**: Minimize external dependencies. New packages
  MUST be justified and compatible with the `net10.0` target.

## Development Workflow

- All changes MUST be scoped to a feature branch named
  `###-feature-name` matching a spec under `specs/`.
- Specifications MUST exist in `specs/###-feature-name/` with at
  minimum `spec.md` and `plan.md` before implementation begins.
- The plan MUST pass a Constitution Check gate (referencing the
  principles above) before proceeding to task generation.
- Implementation follows the task list in `tasks.md`, organized
  by user story priority (P1 first → MVP, then incremental).
- Each pull request MUST include updated or new tests that pass
  before merge.

## Governance

This constitution supersedes ad-hoc practices. All specification
reviews and pull requests MUST verify compliance with the
principles above.

- **Amendments**: Any change to this constitution MUST be
  documented with a version bump, rationale, and migration notes
  for existing specs/tasks that may be affected.
- **Versioning**: Constitution versions follow MAJOR.MINOR.PATCH
  — MAJOR for principle removals or incompatible redefinitions,
  MINOR for new principles or material expansions, PATCH for
  clarifications and wording fixes.
- **Compliance review**: At minimum, the plan-template
  Constitution Check gate MUST reference the current principles.
  Any spec that predates a MAJOR amendment SHOULD be reviewed for
  alignment.
- **Guidance file**: `.github/copilot-instructions.md` serves as
  the runtime development guidance and MUST stay consistent with
  this constitution.

**Version**: 1.0.0 | **Ratified**: 2026-04-08 | **Last Amended**: 2026-04-08
