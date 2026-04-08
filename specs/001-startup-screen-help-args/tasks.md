# Tasks: Startup Screen & Help Argument Listing

**Input**: Design documents from `/specs/001-startup-screen-help-args/`
**Prerequisites**: plan.md (required), spec.md (required), research.md, data-model.md, contracts/cli-contract.md, quickstart.md

**Tests**: Not explicitly requested in the feature specification. Test tasks are omitted.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

---

## Phase 1: Setup (Project Initialization)

**Purpose**: Upgrade all projects to .NET 10 and verify the solution builds cleanly.

- [X] T001 [P] Update TargetFramework from net8.0 to net10.0 in source/ImageBinarizer/ImageBinarizer.csproj
- [X] T002 [P] Update TargetFramework from net8.0 to net10.0 in source/ImageBinarizerTool/ImageBinarizerTool.csproj
- [X] T003 [P] Update TargetFramework from net8.0 to net10.0 in source/ImageBinarizerUnitTest/ImageBinarizerUnitTest.csproj
- [X] T004 [P] Update TargetFramework from net8.0 to net10.0 in source/Sample/Sample.csproj
- [X] T005 [P] Update TargetFramework from net8.0 to net10.0 in source/VideoBinarizerTool/VideoBinarizer.csproj
- [X] T006 Build solution with `dotnet build source/ImageBinarizer.sln` and resolve any .NET 10 compatibility issues

**Checkpoint**: Solution builds cleanly on .NET 10. All existing tests pass.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: No foundational tasks needed — existing project structure and argument parsing infrastructure are already in place. Proceed directly to user stories.

**Checkpoint**: N/A — proceed to Phase 3.

---

## Phase 3: User Story 1 — Application Startup Welcome Screen (Priority: P1) 🎯 MVP

**Goal**: When the user launches `imgbin` with no arguments, display a branded welcome screen showing "ImageBinarizer", version, copyright ©, ASCII art logo, and a help usage hint.

**Independent Test**: Run `dotnet run --project source/ImageBinarizerTool` with no arguments and verify the branded welcome screen appears with all required elements.

### Implementation for User Story 1

- [X] T007 [US1] Update the welcome header in Program.Main() to display "Welcome to ImageBinarizer [version]" with proper © copyright symbol in source/ImageBinarizerTool/Program.cs
- [X] T008 [US1] Replace the current no-args exit path in Program.Main() to display the ASCII logo followed by the help hint "Use \"imgbin -h\" or \"imgbin --help\" for usage information." instead of calling PrintMessage with isError=true in source/ImageBinarizerTool/Program.cs
- [X] T009 [US1] Verify welcome screen output matches the CLI contract (contracts/cli-contract.md §1) by running `dotnet run --project source/ImageBinarizerTool` with no arguments

**Checkpoint**: `imgbin` with no arguments shows branded name, version, ©, ASCII logo, and help hint. Independently testable and deliverable as MVP.

---

## Phase 4: User Story 2 — List All Available Arguments via Help (Priority: P1)

**Goal**: When the user provides `-h` or `--help`, display a complete, structured listing of all 13 arguments with short/long forms, types, defaults, ranges, and usage examples.

**Independent Test**: Run `dotnet run --project source/ImageBinarizerTool -- -h` and verify all 13 arguments are listed in a clean columnar format with types, defaults, and examples.

### Implementation for User Story 2

- [X] T010 [US2] Rewrite the PrintHelp() method in CommandLineParsing.cs to display a header line with application name, version, and description in source/ImageBinarizerTool/CommandLineParsing.cs
- [X] T011 [US2] Add a "Usage: imgbin [options]" line followed by an "Options:" section header with all 13 arguments in aligned columnar format showing short form, long form(s), description, type, default, and range per the CLI contract in source/ImageBinarizerTool/CommandLineParsing.cs
- [X] T012 [US2] Mark required arguments (input image path) with "(required)" label and show default values for optional arguments in the formatted help output in source/ImageBinarizerTool/CommandLineParsing.cs
- [X] T013 [US2] Include usage examples section at the end of PrintHelp() showing common binarization workflows (default, custom dimensions, thresholds, inverse, greyscale, contour, create-code) per the CLI contract in source/ImageBinarizerTool/CommandLineParsing.cs
- [X] T014 [US2] Verify help output matches the CLI contract (contracts/cli-contract.md §2) by running `dotnet run --project source/ImageBinarizerTool -- -h` and `dotnet run --project source/ImageBinarizerTool -- --help`

**Checkpoint**: `imgbin -h` and `imgbin --help` both show all 13 arguments with types, defaults, ranges, required/optional indicators, and usage examples. Independently testable.

---

## Phase 5: User Story 3 — Argument Error Guidance (Priority: P2)

**Goal**: When the user provides invalid or incomplete arguments, display a clear error message followed by a help usage hint.

**Independent Test**: Run `dotnet run --project source/ImageBinarizerTool -- --input-image nonexistent.png --output-image out.txt` and verify the error message is displayed with a help hint.

### Implementation for User Story 3

- [X] T015 [US3] Update the PrintMessage() method in Program.cs to replace the awkward "Insert one of these..." help hint with "Use \"imgbin -h\" or \"imgbin --help\" for usage information." when isError is true in source/ImageBinarizerTool/Program.cs
- [X] T016 [US3] Ensure the error path in Program.Main() displays the welcome header (name + version + copyright) before the error message for consistent branding in source/ImageBinarizerTool/Program.cs
- [X] T017 [US3] Verify error output matches the CLI contract (contracts/cli-contract.md §4) by running with invalid arguments (nonexistent file, out-of-range threshold, negative dimensions)

**Checkpoint**: Invalid arguments produce a clear error message + help hint. All 3 user stories now work independently.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final validation across all stories and cleanup.

- [X] T018 Run `dotnet build source/ImageBinarizer.sln` to confirm clean build with no warnings
- [X] T019 Run `dotnet test source/ImageBinarizerUnitTest` to confirm all existing unit tests pass
- [X] T020 Run quickstart.md verification steps (all 6 checks) to validate end-to-end behavior

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies — can start immediately
- **Foundational (Phase 2)**: N/A — no foundational tasks needed
- **User Story 1 (Phase 3)**: Depends on Setup (Phase 1) completion — edits Program.cs
- **User Story 2 (Phase 4)**: Depends on Setup (Phase 1) completion — edits CommandLineParsing.cs. Can run in parallel with US1 (different files)
- **User Story 3 (Phase 5)**: Depends on User Story 1 (Phase 3) completion — edits Program.cs (same file as US1)
- **Polish (Phase 6)**: Depends on all user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Setup — modifies `Program.cs`
- **User Story 2 (P1)**: Can start after Setup — modifies `CommandLineParsing.cs` — **parallelizable with US1** (different file)
- **User Story 3 (P2)**: Must wait for User Story 1 — modifies `Program.cs` (same file, builds on US1 changes to header/branding)

### Within Each User Story

- Implementation tasks within the same file are sequential
- Verification task is always last in each story

### Parallel Opportunities

- T001–T005 (all .csproj updates) can run in parallel
- User Story 1 (T007–T009) and User Story 2 (T010–T014) can run in parallel (different files)

---

## Parallel Example: User Stories 1 & 2

```
# After Setup (Phase 1) completes, launch both stories in parallel:

# Stream A: User Story 1 (Program.cs)
Task T007: Update welcome header in Program.cs
Task T008: Replace no-args exit path in Program.cs
Task T009: Verify welcome screen output

# Stream B: User Story 2 (CommandLineParsing.cs)
Task T010: Rewrite PrintHelp() header
Task T011: Add aligned argument listing
Task T012: Mark required/optional with defaults
Task T013: Add usage examples
Task T014: Verify help output

# After both complete → User Story 3 (edits Program.cs again)
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup (.NET 10 migration)
2. Complete Phase 3: User Story 1 (welcome screen)
3. **STOP and VALIDATE**: Run `imgbin` with no arguments — branded screen appears
4. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup → .NET 10 builds cleanly
2. Add User Story 1 → Welcome screen works → MVP!
3. Add User Story 2 → Help listing works → Core feature complete
4. Add User Story 3 → Error guidance works → Full feature complete
5. Polish → All tests pass, clean build

### Parallel Strategy

1. Complete Setup together
2. Once Setup is done:
   - Stream A: User Story 1 (Program.cs)
   - Stream B: User Story 2 (CommandLineParsing.cs)
3. After both complete: User Story 3 (Program.cs, builds on US1)
4. Polish phase validates everything

---

## Notes

- [P] tasks = different files, no dependencies between them
- [Story] label maps task to specific user story for traceability
- Tests were NOT explicitly requested — omitted per task generation rules
- Total: 20 tasks across 6 phases
- Each user story is independently completable and testable
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
