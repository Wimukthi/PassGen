# Dark Mode Implementation Plan for PassGen

This document outlines the plan to implement a dark mode feature for the PassGen VB.NET WinForms application.

## Requirements

*   Allow users to switch between Light, Dark, and System themes.
*   Persist the user's theme choice across application sessions.
*   Use standard dark theme colors (dark gray backgrounds, light text).
*   Attempt to follow the Windows theme when "System" mode is selected.

## Plan Details

1.  **Define Color Palettes:**
    *   **Light Mode:** Based on current defaults (e.g., `SystemColors.Control`, `SystemColors.Window`, `SystemColors.ControlText`).
    *   **Dark Mode:** Standard dark theme (e.g., `Color.FromArgb(45, 45, 48)` for backgrounds, `Color.White` or `Color.LightGray` for text).
    *   **System Mode:** Logic to detect the current Windows theme (light or dark) and apply the corresponding palette.

2.  **Create `ThemeManager`:**
    *   A central class/module to manage theming logic.
    *   Store Light, Dark, and System theme settings.
    *   Include logic to detect the Windows theme (potentially using .NET 8 APIs or P/Invoke if needed).
    *   Implement `ApplyTheme(Control container)` to recursively style controls based on the active theme.
    *   Include specific logic to handle controls requiring special styling (e.g., `ListView`, `ProgressBar`).

3.  **Implement Theme Switching:**
    *   Add a "View" top-level menu item to `frmMain`.
    *   Add a "Theme" submenu under "View".
    *   Add radio-button-style menu items for "Light", "Dark", and "System" within the "Theme" submenu.
    *   When a theme menu item is selected:
        *   Update the `ThemeManager`'s active theme setting.
        *   Call the `ApplyTheme` function for all open forms.
        *   Save the user's *choice* (Light/Dark/System) using `My.Settings`.

4.  **Integrate Theming into Forms:**
    *   In the `Load` event of each form (`frmMain`, `frmAbout`):
        *   Load the saved theme preference from `My.Settings`.
        *   Determine the effective theme (if "System" is chosen, detect the OS theme).
        *   Apply the initial theme using the `ThemeManager`.
    *   Listen for OS theme change events (if implementing System mode reliably) to update the UI dynamically when the "System" option is active.
    *   Review and potentially remove hardcoded colors in designer files if they conflict with the theme manager's logic.

5.  **Handle Specific Controls:**
    *   **`ListView` (`lstvKeys`):** May require `OwnerDraw = True` and manual drawing for proper coloring in both modes. Investigate default `BackColor`/`ForeColor` first.
    *   **`TextBox`, Labels styled as TextBoxes:** Ensure `BackColor`, `ForeColor`, and `BorderStyle` are correctly themed.
    *   **`ProgressBar`:** Standard controls are hard to theme; may need to accept default appearance or use custom controls.
    *   **Buttons, Checkboxes, etc.:** Apply consistent colors from the palettes.

## Visual Representation (Mermaid Diagram)

```mermaid
graph TD
    subgraph User Interaction
        A[User Action: Select Theme Menu (Light/Dark/System)] --> B{Update ThemeManager State};
    end

    subgraph Theme Management
        B --> C{Persist Setting (My.Settings)};
        B --> D[Apply Theme to All Open Forms];
        D --> E[ThemeManager.ApplyTheme(Form)];
        E -- Iterates through --> F(Controls on Form);
        F -- Sets --> G[BackColor, ForeColor, etc.];
        G -- Handles --> H(Special Controls e.g., ListView);

        ThemeManagerClass[ThemeManager Class]:::TMClass
        ThemeManagerClass -- Contains --> CurrentThemeSetting((User Setting: Light/Dark/System))
        ThemeManagerClass -- Contains --> Palettes{Light & Dark Palettes}
        ThemeManagerClass -- Contains --> ApplyMethod(ApplyTheme Method)
        ThemeManagerClass -- Contains --> DetectOSTheme(Detect OS Theme Method)
        ThemeManagerClass -- Contains --> ListenOSTheme(Listen for OS Theme Change)

        style ThemeManagerClass fill:#f9f,stroke:#333,stroke-width:2px
        classDef TMClass fill:#f9f,stroke:#333,stroke-width:2px
    end

    subgraph Application Lifecycle
        I[App Startup] --> J{Load Saved Setting (My.Settings)};
        J --> K[Set Initial ThemeManager State];
        K -- If System Setting --> L[DetectOSTheme];
        L --> D;
        K -- If Light/Dark Setting --> D;
        M[OS Theme Change Event] -- If System Setting Active --> L;
    end

    C --> J;