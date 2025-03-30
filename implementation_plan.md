# Refactoring Plan for PassGen

## Current Structure Analysis

Based on the review of the codebase (`frmMain.vb`, `PasswordStrength.vb`, `Calc.vb`, `MD5.vb`, `SHA.vb`, `ErrorLogger.vb`), the current structure has the following characteristics:

1.  **`frmMain.vb`:** Acts as the central hub, containing UI event handling, password generation logic (using `BackgroundWorker`), entropy calculation initiation (using another `BackgroundWorker`), calls to hashing functions, and file saving logic. This leads to a large, tightly coupled class.
2.  **`PasswordStrength.vb`:** Calculates entropy but delegates the actual math evaluation (`Length * log(UniqueChars) / log(2)`) to `mcCalc` found in `Calc.vb`.
3.  **`Calc.vb`:** Contains a complex, general-purpose math expression evaluator (`mcCalc`) which is overly sophisticated for its sole use case: calculating the specific entropy formula.
4.  **`MD5.vb`:** A simple wrapper for the standard .NET `MD5CryptoServiceProvider`.
5.  **`SHA.vb`:** Handles SHA1, SHA256, SHA384, and SHA512. **Crucially, it automatically generates and adds a random salt when hashing if no salt is provided.** This causes the SHA hashes displayed in the UI to change on every generation for the same password, which is inconsistent with the unsalted MD5 hash and potentially confusing for the user in this context.
6.  **`ErrorLogger.vb`:** A basic class to log errors to a text file (`Error_Log.txt`) in the application's `Errors` subdirectory.

## Refactoring Goals

*   **Improve Code Organization:** Separate distinct responsibilities (UI, generation, calculation, hashing, saving) into dedicated classes following the Single Responsibility Principle.
*   **Simplify Calculations:** Replace the complex math evaluator in `Calc.vb` with direct .NET `Math.Log` functions for entropy calculation.
*   **Ensure Consistent Hashing:** Modify the SHA hashing to be *unsalted* for display purposes, making the output deterministic and consistent with the MD5 hash display. (Note: Salting is vital for password *storage*, but less appropriate for simply displaying representative hashes in this tool).
*   **Maintain Functionality:** Ensure all existing features (password generation with options, entropy calculation, hash display, saving) work correctly after refactoring.
*   **Enhance Maintainability & Testability:** Make the code easier to understand, modify, and potentially unit test in the future by decoupling components.

## Proposed Refactoring Plan

1.  **Create New Service Classes:**
    *   `PasswordGenerator.vb`: Encapsulate character set logic and secure random password string generation.
    *   `EntropyCalculator.vb`: Calculate entropy directly using the formula `Length * Math.Log(UniqueChars) / Math.Log(2)`.
    *   `HashingService.vb`: Provide methods for generating *unsalted* MD5, SHA256, and SHA512 hashes (returning Base64 strings). This will replace `MD5.vb` and `SHA.vb`.
    *   `PasswordSaver.vb`: Handle the logic for saving the list of generated passwords and their hashes to a text file, matching the existing format.
2.  **Refactor `frmMain.vb`:**
    *   Instantiate and use the new service classes (`PasswordGenerator`, `EntropyCalculator`, `HashingService`, `PasswordSaver`).
    *   Delegate the core logic for password generation, entropy calculation, hashing, and saving tasks to the respective service classes.
    *   Keep UI update logic (updating text boxes, list view, progress bars) within the form's event handlers (like `bwgen_RunWorkerCompleted`, `threadEntropy_RunWorkerCompleted`, `lstvKeys_SelectedIndexChanged`).
    *   Continue using `BackgroundWorker` instances (`bwgen`, `threadEntropy`) for managing asynchronous operations and UI updates, but have their `DoWork` handlers call methods in the new service classes.
3.  **Remove Redundant/Overcomplex Code:**
    *   Delete `Calc.vb` as the math evaluator is no longer needed.
    *   Delete `MD5.vb` and `SHA.vb` as their functionality will be consolidated and improved in `HashingService.vb`.
    *   Delete `PasswordStrength.vb` as its logic will move into `EntropyCalculator.vb`.
4.  **Keep `ErrorLogger.vb`:** Retain the existing error logging mechanism for now.
5.  **Testing:** Perform thorough manual testing after the refactoring to verify all features remain functional and the SHA hashes are now consistent and deterministic for display.

## Visualized Structure (Post-Refactoring)

```mermaid
graph TD
    subgraph UI Layer
        frmMain(frmMain.vb)
    end

    subgraph Core Services
        PasswordGenerator(PasswordGenerator.vb)
        EntropyCalculator(EntropyCalculator.vb)
        HashingService(HashingService.vb)
        PasswordSaver(PasswordSaver.vb)
    end

    subgraph Utilities
        ErrorLogger(ErrorLogger.vb)
    end

    frmMain -- Uses --> PasswordGenerator
    frmMain -- Uses --> EntropyCalculator
    frmMain -- Uses --> HashingService
    frmMain -- Uses --> PasswordSaver
    frmMain -- Uses --> ErrorLogger
    PasswordGenerator -- Uses --> ErrorLogger
    EntropyCalculator -- Uses --> ErrorLogger
    HashingService -- Uses --> ErrorLogger
    PasswordSaver -- Uses --> ErrorLogger

    classDef ui fill:#f9f,stroke:#333,stroke-width:2px;
    classDef core fill:#ccf,stroke:#333,stroke-width:2px;
    classDef util fill:#cfc,stroke:#333,stroke-width:2px;

    class frmMain ui;
    class PasswordGenerator,EntropyCalculator,HashingService,PasswordSaver core;
    class ErrorLogger util;