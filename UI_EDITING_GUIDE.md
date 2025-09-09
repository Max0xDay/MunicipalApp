# UI Editing Guide for MunicipalApp

## Main Files to Edit:

### 1. **Main Windows**
- `Views/MainWindow.axaml` - Main application window
- `Views/ReportWizardForm.axaml` - Wizard container with progress bar

### 2. **Wizard Pages (Order matters)**
- `Views/WelcomePage.axaml` - Welcome screen
- `Views/LocationPage.axaml` - Location input with autocomplete
- `Views/CategoryPage.axaml` - Category and description
- `Views/PhotoPage.axaml` - Photo upload
- `Views/ConfirmationPage.axaml` - Success screen

### 3. **Admin Interface**
- `Views/AdminReportForm.axaml` - Admin dashboard

### 4. **Styles**
- `Styles/AppStyles.axaml` - Colors, buttons, text styles

## Common Editing Tasks:

### **Moving Elements Around**
1. Open the `.axaml` file in Visual Studio
2. Switch to **Design** view
3. Select the element you want to move
4. Drag it to the new position
5. Use the Properties panel to adjust margins/padding

### **Changing Colors**
1. Select the element
2. In Properties panel, find:
   - `Background` - For background colors
   - `Foreground` - For text colors
   - `BorderBrush` - For border colors

### **Resizing Elements**
1. Select the element
2. Drag the corner handles OR
3. Use Properties panel to set exact:
   - `Width` and `Height`
   - `Margin` (spacing outside)
   - `Padding` (spacing inside)

### **Editing Text**
1. Select the TextBlock or Button
2. In Properties panel, find `Text` property
3. Type your new text

### **Adding New Elements**
1. Open Toolbox (View → Toolbox)
2. Drag controls like:
   - `TextBlock` - For text labels
   - `Button` - For clickable elements
   - `TextBox` - For text input
   - `Border` - For containers with backgrounds

## Quick Tips:

- **Grid Layout**: Use for precise positioning with rows/columns
- **StackPanel**: Use for stacking elements vertically/horizontally
- **Border**: Use to add background colors and borders
- **Margin**: Space outside an element
- **Padding**: Space inside an element
- **HorizontalAlignment**: Left/Center/Right/Stretch
- **VerticalAlignment**: Top/Center/Bottom/Stretch

## Hot Reload:
1. Run your application with `dotnet run`
2. Keep it running
3. Edit XAML files in Visual Studio
4. Changes appear instantly in the running app!

## Keyboard Shortcuts (VS):
- `F4` - Properties panel
- `Ctrl+Alt+X` - Toolbox
- `Ctrl+Shift+B` - Build
- `F5` - Run with debugging