# Municipal Services Application - Part 1

## Overview

This is a South African Municipal Services Application built with .NET Framework 4.7.2 Windows Forms. The application allows citizens to report municipal issues and provides an admin interface to view all reported issues.

## Features Implemented (Part 1)

### Main Menu
- Three navigation buttons: "Report Issues", "Local Events and Announcements", "Service Request Status"
- Only "Report Issues" is enabled (others disabled for Part 1 as per assignment requirements)
- Clean, professional interface

### Report Issues Form
- **Location Input**: TextBox for entering issue location
- **Category Selection**: ComboBox with options (Sanitation, Roads, Utilities, Water, Electricity)
- **Description**: RichTextBox for detailed issue description
- **Media Attachment**: Button to browse and attach files (images, documents)
- **Progress Bar**: Shows completion percentage at bottom (key user engagement feature)
- **Submit Button**: Saves report to SQLite database
- **Admin Button**: Access to admin view
- **Social Media Buttons**: WhatsApp, Email, SMS (placeholder buttons as required)

### Admin Form
- DataGridView displaying all reported issues
- Shows ID, Location, Category, Description, Attachment, and Report Date
- Refresh button to reload data
- Professional layout with proper formatting

### Data Management
- **SQLite Database**: Persistent storage for all reported issues
- **List<Issue> Data Structure**: In-memory management of issues as required by assignment
- **Issue Class**: Proper data model with Id, Location, Category, Description, AttachmentPath, ReportDate
- **Input Validation**: Ensures all required fields are filled
- **Error Handling**: Proper exception handling for database operations

## User Engagement Strategy Implementation

The application implements a **Progress Bar** user engagement strategy:
- Real-time progress tracking as users complete form fields
- Progress bar at bottom shows 0-100% completion
- Visual feedback encourages users to complete all fields
- Provides clear indication of form completion status

## Technical Requirements Met

✅ **Main Menu with 3 tasks** (Report Issues enabled, others disabled)  
✅ **Report Issues functionality** with all required input fields  
✅ **Media attachment** using OpenFileDialog  
✅ **User engagement strategy** (progress bar)  
✅ **Appropriate data structures** (List<Issue>)  
✅ **SQLite database** for persistent storage  
✅ **Admin interface** to view all reports  
✅ **Social media buttons** (non-functional placeholders)  
✅ **Clean, professional UI** with consistent design  
✅ **Input validation and error handling**  

## How to Compile and Run

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2
- System.Data.SQLite NuGet package

### Compilation Steps
1. Open `Sidequest municiple app.sln` in Visual Studio
2. Restore NuGet packages (System.Data.SQLite will be downloaded automatically)
3. Build the solution (Build → Build Solution or Ctrl+Shift+B)
4. Run the application (Debug → Start Debugging or F5)

### Alternative Compilation (Command Line)
```bash
# Navigate to project directory
cd "D:\.projects\Sidequest municiple app"

# Restore NuGet packages
nuget restore

# Build using MSBuild
msbuild "Sidequest municiple app.sln" /p:Configuration=Debug
```

### Running the Application
1. Execute the compiled .exe from `bin\Debug\` folder
2. Application starts with the Main Menu
3. Click "Report Issues" to submit new issues
4. Use the "Admin" button to view all submitted reports

## Usage Instructions

### Reporting an Issue
1. Launch the application
2. Click "Report Issues" from the main menu
3. Fill in the required fields:
   - **Location**: Enter the location of the issue
   - **Category**: Select from dropdown (Sanitation, Roads, Utilities, Water, Electricity)
   - **Description**: Provide detailed description of the issue
   - **Attachment** (optional): Browse and select a file to attach
4. Watch the progress bar fill as you complete fields
5. Click "Submit Report" to save the issue
6. A success message will show the unique reference ID

### Viewing Reports (Admin)
1. From the Report Issues form, click the "Admin" button
2. All submitted reports are displayed in a data grid
3. Click "Refresh" to reload the latest data
4. Reports show ID, location, category, description, attachment status, and date

### Social Media Sharing
- Social media buttons (WhatsApp, Email, SMS) are present but show "not implemented" message
- These are placeholder buttons as required by the assignment

## Data Storage

- **Database**: SQLite database file created in `Documents\MunicipalApp.db`
- **Issues Table**: Stores Id, Location, Category, Description, AttachmentPath, ReportDate
- **File Attachments**: File paths are stored in database, actual files remain in original location

## Code Standards

- **No code comments** (as required by assignment)
- **No emojis** in code or UI
- Clean, readable code structure
- Proper C# naming conventions
- Comprehensive error handling
- Professional UI design

## Assignment Compliance

This implementation fully meets all Part 1 requirements:
- ✅ Main menu with three options (only Report Issues enabled)
- ✅ Report Issues form with all required elements
- ✅ User engagement strategy implementation (progress bar)
- ✅ Appropriate data structures (List<Issue>)
- ✅ Media attachment functionality
- ✅ Professional, consistent UI design
- ✅ SQLite database integration
- ✅ Admin functionality to view reports
- ✅ Social media sharing buttons (placeholders)

The application is ready for demonstration and meets all technical and functional requirements specified in the assignment.