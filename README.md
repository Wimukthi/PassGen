# PassGen

## Overview

PassGen is a Windows Forms application built with VB.NET and .NET 8.0. It provides tools for generating secure passwords, calculating their entropy (strength), and generating corresponding cryptographic hashes.

## Features

*   **Customizable Password Generation:** Generate passwords with user-defined length and character sets (uppercase, lowercase, numbers, special symbols, space, custom characters).
*   **Batch Generation:** Efficiently generate multiple passwords at once using background workers to keep the UI responsive. Includes progress indication.
*   **Entropy Calculation:** Calculates password entropy based on length and character set size, providing a strength assessment (Very Weak, Weak, Moderate, Strong, Very Strong).
*   **Hashing:** Generates MD5, SHA256, and SHA512 hashes for each generated password.
*   **Save Functionality:** Save the list of generated passwords, entropy details, and hashes to a text file.
*   **Clipboard Integration:** Easily copy generated hashes (MD5, SHA256, SHA512) to the clipboard.
*   **Error Logging:** Includes basic file-based error logging for troubleshooting.

## Technology Stack

*   **Language:** Visual Basic .NET (VB.NET)
*   **Framework:** .NET 8.0 (Windows)
*   **UI:** Windows Forms (WinForms)

## Prerequisites

*   **Operating System:** Windows
*   **SDK:** .NET 8.0 SDK (Software Development Kit)
*   **IDE:** Visual Studio (e.g., 2022 or later recommended) with the ".NET desktop development" workload installed.

## Building and Running

1.  **Clone the repository:**
    ```bash
    git clone <repository-url> 
    # Replace <repository-url> with the actual URL of this repository
    cd PassGen 
    ```
2.  **Open in Visual Studio:** Open the `PassGen.sln` file.
3.  **Build:** Ensure the .NET 8.0 SDK is installed and recognized. Build the solution using the menu (Build > Build Solution) or shortcut (Ctrl+Shift+B).
4.  **Run:** Start the application using the menu (Debug > Start Debugging) or shortcut (F5).
5.  **Executable Location:** The compiled executable can typically be found in `PassGen/bin/[Configuration]/net8.0-windows/` (e.g., `PassGen/bin/Debug/net8.0-windows/PassGen.exe`).

## Code Structure Overview

*   `PassGen.sln`: Visual Studio Solution file.
*   `PassGen/`: Main project directory.
    *   `PassGen.vbproj`: The project file (.NET 8 SDK-style). Defines target framework, dependencies, and included files.
    *   `frmMain.vb`: Code-behind for the main application window. Handles UI events, orchestrates calls to services, and manages background workers for generation.
    *   `frmAbout.vb`: Code-behind for the simple 'About' dialog.
    *   `PasswordGenerator.vb`: Service class responsible for building character sets and generating password strings.
    *   `EntropyCalculator.vb`: Service class for calculating password entropy.
    *   `HashingService.vb`: Service class encapsulating MD5, SHA256, and SHA512 hash generation logic.
    *   `PasswordSaver.vb`: Service class for saving the generated password data list to a text file.
    *   `ErrorLogger.vb`: Service class providing basic file-based error logging capabilities.
    *   `My Project/`: Folder containing VB.NET project-specific files:
        *   `Application.myapp`: Application framework settings.
        *   `AssemblyInfo.vb`: Assembly metadata.
        *   `Resources.resx`/`.Designer.vb`: Embedded application resources.
        *   `Settings.settings`/`.Designer.vb`: Application settings.
        *   `app.manifest`: Application manifest file (controls UAC, etc.).
    *   `Resources/`: Folder containing image resources used by the UI.
    *   `Key.ico`: Application icon file.
*   `implementation_plan.md`: Document detailing the batch generation performance optimization.
*   `gpl-3.0.rtf`: The full text of the GNU General Public License v3.0.
*   `.gitignore`, `.gitattributes`: Git configuration files.

*(Note: `Program.vb` is typically excluded in modern VB.NET WinForms projects using the Application Framework, where the entry point is managed via project settings pointing to `MainForm`)*

## Usage Overview (Developer Focus)

The application adopts a service-oriented pattern within the WinForms framework. `frmMain` acts as the primary controller/presenter. On load, it instantiates the necessary service classes (`PasswordGenerator`, `EntropyCalculator`, `HashingService`, `PasswordSaver`, `ErrorLogger`).

User interactions (button clicks, checkbox changes) trigger event handlers within `frmMain`. These handlers validate input and delegate the core logic to the appropriate service methods.

For the potentially time-consuming task of generating multiple passwords, `frmMain` utilizes a `BackgroundWorker` component (`bwgen`). This performs the generation loop (calling `PasswordGenerator`, `EntropyCalculator`, `HashingService` for each password) on a separate thread. Results are collected in a `List(Of PasswordData)` structure. Progress is reported back to the UI thread to update a progress bar. Upon completion, the `RunWorkerCompleted` event handler efficiently updates the `ListView` control using `Items.AddRange` to avoid UI freezes.

## Known Issues / Future Plans

*   The batch password generation process was recently optimized for performance. Details can be found in `implementation_plan.md`.
*   No other specific known issues or future enhancements are documented within the code comments at this time.

## License

This project is licensed under the **GNU General Public License v3.0**.

A copy of the license is included in the file `gpl-3.0.rtf`. You can also find it online at [https://www.gnu.org/licenses/gpl-3.0.html](https://www.gnu.org/licenses/gpl-3.0.html).