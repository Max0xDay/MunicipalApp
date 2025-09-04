# Municipal Services Application - Part 1

A Windows Forms application for South African municipal services that allows citizens to report issues and request services.

## Features

### Part 1 - Report Issues (Implemented)
- **Main Menu**: Navigation hub with options for Report Issues, Local Events (Part 2), and Service Request Status (Part 3)
- **Issue Reporting**: Complete form for reporting municipal issues with:
  - Location input field
  - Category selection (Sanitation, Roads, Utilities, Water, Electricity)
  - Rich text description box
  - File attachment capability
  - Real-time progress tracking with visual feedback
  - Form validation and error handling
  - JSON data persistence

### User Engagement Features
- **Progress Bar**: Shows completion percentage (0-100%) as user fills form
- **Dynamic Messages**: Encouraging feedback based on progress
- **Visual Feedback**: Submit button changes color when form is complete
- **Step Counter**: Shows "X of Y fields completed"

## System Requirements

- Windows 10 or later
- .NET 8.0 Runtime
- Minimum 4GB RAM
- 100MB free disk space

## How to Compile and Run

### Prerequisites
Ensure you have the .NET 8.0 SDK installed:
```bash
dotnet --version
```

### Compilation
1. Open Command Prompt or PowerShell
2. Navigate to the project directory:
   ```bash
   cd "D:\.projects\MunicipalApp"
   ```
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Build the application:
   ```bash
   dotnet build
   ```

### Running the Application
Run directly from source:
```bash
dotnet run
```

Or build and run the executable:
```bash
dotnet build --configuration Release
cd bin\Release\net8.0-windows
MunicipalApp.exe
```

## How to Use

### Reporting an Issue
1. Launch the application
2. Click "Report Issues" on the main menu
3. Fill in the required information:
   - **Location**: Enter the specific location of the issue
   - **Category**: Select from dropdown (Sanitation, Roads, Utilities, Water, Electricity)
   - **Description**: Provide detailed description of the issue
   - **Attachments**: Optionally attach photos or documents
4. Watch the progress bar and encouraging messages as you complete fields
5. Click "Submit Issue" when all fields are completed
6. Your issue will be saved with a unique reference ID

### Data Storage
- Issues are automatically saved to `Data/issues.json`
- File attachments are referenced by their original file paths
- Each issue receives a unique ID and timestamp

### Navigation
- Use "Back" button to return to main menu
- Disabled menu options will be enabled in future parts
- Close application using the X button or Alt+F4

## Project Structure
```
MunicipalApp/
├── Models/
│   └── Issue.cs              # Issue data model
├── Services/
│   └── DataService.cs        # JSON data persistence
├── Forms/
│   ├── MainMenuForm.cs       # Main navigation
│   └── ReportIssuesForm.cs   # Issue reporting form
├── Data/
│   └── issues.json           # Stored issues (created automatically)
└── README.md                 # This file
```

## Technical Implementation

### Technologies Used
- .NET 8.0 Windows Forms
- Newtonsoft.Json for data serialization
- OpenFileDialog for file attachments
- Real-time form validation

### Data Structures
- `List<Issue>` for storing reported issues
- `List<string>` for file attachment paths
- JSON serialization for data persistence

### User Engagement Strategy
The application implements **Real-time Feedback and Progress Tracking Systems**:
- Visual progress indicators show completion status
- Dynamic encouraging messages motivate users
- Color-coded feedback (gray/green) indicates form state
- Success confirmation with issue reference ID

## Future Development (Parts 2 & 3)

### Part 2 - Local Events and Announcements
- Advanced data structures (Stacks, Queues, Dictionaries, Sets)
- Event search and recommendation system

### Part 3 - Service Request Status  
- Complex data structures (Trees, Heaps, Graphs)
- Service request tracking and status management

## Troubleshooting

### Common Issues
- **Application won't start**: Ensure .NET 8.0 Runtime is installed
- **Files not attaching**: Check file permissions and paths
- **Data not saving**: Verify write permissions in application directory

### Error Messages
- Form validation errors will display specific field requirements
- File operation errors will show detailed messages
- Data persistence errors are logged and displayed to user

## Support
For technical support or questions about this application, please refer to the project documentation or contact the development team.

---
*Built with .NET 8.0 Windows Forms*