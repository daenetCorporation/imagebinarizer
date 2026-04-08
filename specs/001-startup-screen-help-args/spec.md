# Feature Specification: Startup Screen & Help Argument Listing

**Feature Branch**: `001-startup-screen-help-args`  
**Created**: 2026-04-08  
**Status**: Draft  
**Input**: User description: "When user starts the application a nice screen with the application name 'ImageBinarizer' appears. The application provides the start argument --arg that lists all available arguments."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Application Startup Welcome Screen (Priority: P1)

As a user, when I launch the ImageBinarizer application without any arguments, I want to see an attractive branded welcome screen that clearly displays the application name "ImageBinarizer", the current version number, and a copyright notice, so I know the application has started correctly and I can identify which version I am running.

**Why this priority**: The startup screen is the user's first impression of the application. It builds trust, confirms the application is running, and provides essential version information. This is also the simplest standalone deliverable.

**Independent Test**: Can be fully tested by launching `imgbin` with no arguments and verifying the branded screen appears with the application name, version, and copyright.

**Acceptance Scenarios**:

1. **Given** the application is installed, **When** the user launches `imgbin` with no arguments, **Then** a welcome screen is displayed showing the application name "ImageBinarizer", the current version number, and a copyright notice.
2. **Given** the application is installed, **When** the user launches `imgbin` with no arguments, **Then** the welcome screen includes a visually distinctive logo or ASCII art representation of the application brand.
3. **Given** the application is installed, **When** the user launches `imgbin` with no arguments, **Then** a hint is displayed informing the user how to access help (e.g., `imgbin -h` or `imgbin --help`).

---

### User Story 2 - List All Available Arguments via Help (Priority: P1)

As a user, when I provide the help argument (`-h` or `--help`), I want to see a complete, well-organized listing of every available command-line argument, including short and long forms, descriptions, expected value types, and default values, so I can understand how to use the application without consulting external documentation.

**Why this priority**: Discoverability of arguments is essential for usability. Without this, users cannot effectively use the binarization features. This is equally critical to the startup screen.

**Independent Test**: Can be fully tested by running `imgbin -h` or `imgbin --help` and verifying that all arguments are listed with their descriptions.

**Acceptance Scenarios**:

1. **Given** the application is installed, **When** the user runs `imgbin -h`, **Then** a help screen is displayed listing all available arguments.
2. **Given** the application is installed, **When** the user runs `imgbin --help`, **Then** the same help screen is displayed as with `-h`.
3. **Given** the help screen is displayed, **When** the user reads the output, **Then** each argument entry shows: short form, long form, description, and expected value type or range.
4. **Given** the help screen is displayed, **When** the user reads the output, **Then** the following arguments are all listed:
   - Input image path (`-ip`, `--input-image`)
   - Output image path (`-op`, `--output-image`)
   - Image width (`-iw`, `--imageWidth`)
   - Image height (`-ih`, `--imageHeight`)
   - Red threshold (`-rt`, `--redThreshold`)
   - Green threshold (`-gt`, `--greenThreshold`)
   - Blue threshold (`-bt`, `--blueThreshold`)
   - Grey threshold (`-grt`, `--greyThreshold`)
   - Inverse mode (`-inv`, `--inverse`)
   - Grey scale mode (`-gs`, `--greyscale`)
   - Create code (`-cc`, `--createcode`, `--create-code`)
   - Get contour (`-gc`, `--getcontour`)
   - Help (`-h`, `--help`)
5. **Given** the help screen is displayed, **When** the user reads the output, **Then** usage examples are provided showing common usage patterns.

---

### User Story 3 - Argument Error Guidance (Priority: P2)

As a user, when I provide invalid or incomplete arguments, I want to see a clear error message along with a hint to use the help argument, so I can quickly correct my command without frustration.

**Why this priority**: Error guidance improves the user experience and reduces support burden, but relies on the help system (Story 2) being in place first.

**Independent Test**: Can be fully tested by running `imgbin` with an invalid argument and verifying the error message includes guidance to use help.

**Acceptance Scenarios**:

1. **Given** the application is installed, **When** the user runs `imgbin` with an unrecognized argument, **Then** an error message is displayed describing the issue.
2. **Given** an error message is displayed, **When** the user reads the output, **Then** the message includes a hint to use `-h` or `--help` for full argument listing.
3. **Given** the application is installed, **When** the user runs `imgbin --input-image nonexistent.png --output-image out.txt`, **Then** a clear error message states that the input file does not exist, along with the help hint.

---

### Edge Cases

- What happens when the user provides arguments with incorrect value types (e.g., text instead of a number for threshold)?
- How does the application behave when both short and long forms of the same argument are provided simultaneously?
- What happens when threshold values are outside the valid range (below 0 or above 255)?
- What happens when width or height is set to a negative number?
- How does the application respond when the output directory does not exist?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The application MUST display a branded welcome screen showing "ImageBinarizer", the current version number, and a copyright notice when launched with no arguments.
- **FR-002**: The application MUST display a visually distinctive logo or ASCII art as part of the welcome screen.
- **FR-003**: The application MUST display a help hint on the welcome screen directing users to use `-h` or `--help` for argument information.
- **FR-004**: The application MUST display a complete argument listing when the user provides `-h` or `--help`.
- **FR-005**: The help listing MUST include all 13 supported arguments: input image path, output image path, image width, image height, red threshold, green threshold, blue threshold, grey threshold, inverse mode, grey scale mode, create code, get contour, and help.
- **FR-006**: Each argument in the help listing MUST show its short form, long form(s), a description of its purpose, and the expected value type or valid range.
- **FR-007**: The help listing MUST include usage examples demonstrating common binarization workflows (default binarization, custom dimensions, threshold-based, inverse, grey scale, contour extraction, and code generation).
- **FR-008**: The application MUST display a clear error message when invalid or missing arguments are provided, followed by a hint to use the help argument.
- **FR-009**: The application MUST indicate which arguments are required (input image path) versus optional.
- **FR-010**: The help listing MUST indicate default values for optional arguments (e.g., threshold defaults to auto-detect when set to -1, dimensions default to original size when set to 0).

### Key Entities

- **Argument**: A command-line parameter the user can provide. Has a short form (e.g., `-ip`), one or more long forms (e.g., `--input-image`), a description, a value type (string, integer, or boolean flag), a valid range (where applicable), and a default value.
- **Welcome Screen**: The initial display shown when the application launches without arguments. Contains the application name, version, logo, copyright, and help hint.
- **Help Screen**: The display shown when the help argument is invoked. Contains the full argument listing, descriptions, and usage examples.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% of supported arguments are listed in the help output with accurate descriptions and value ranges.
- **SC-002**: Users can discover all available arguments within 10 seconds of launching the help command.
- **SC-003**: The welcome screen displays the application name, version, and copyright within 1 second of application launch.
- **SC-004**: Users who encounter an argument error can find the correct syntax by following the displayed help hint without any external documentation.
- **SC-005**: All 13 arguments are documented in the help output with both short and long forms, matching the actual behavior of the application.

## Assumptions

- The application is a command-line tool invoked via `imgbin` and does not have a graphical user interface.
- The existing ASCII art logo in the codebase will continue to be used as the visual brand element on the welcome screen.
- The welcome screen and help output are displayed to the standard console output (stdout), with errors going to the console as well.
- The argument listing reflects all currently implemented arguments in the application and will be updated as new arguments are added.
- The `--arg` notation mentioned in the user request maps to the existing `-h` / `--help` argument that already provides argument listing functionality.
