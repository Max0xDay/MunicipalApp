# Municipal Services Application

---

## Important Notice

This project has an attached pdf document aswell as a fully written outline of every single data type and data structure in the HTML file ([DataStructures_Documentation.html](/Submission/DataStructures_Documentation.html)). I urge you please to read it because I have implemented every single type of data structure in this project including extra ones from my own technical knowledge. My part two submission has sections which were not marked. The html document outlines all the in-depth technical explanations as well as where to find each data structure and algorithm in the code.

It is also worth noting that a GitHub Classroom was given only in the late stages of this project, so all commits and development histories are in [MunicipalApp](https://github.com/Max0xDay/MunicipalApp) while the submission URL is [prog7312-poe-ML0Day0VC](https://github.com/VCSTDN2024/prog7312-poe-ML0Day0VC).

I'm so sorry for the inconvenience, but I really appreciate it.

### Submission Documents

The folder **Submission** contains the following documents for easy review:

- **[Complete Project Report (PDF)](Submission/Max%20Day%20ST10322238%20Final%20PROG7312%20Submission%20Document.pdf)** - Full project documentation and report
- **[Data Structures Documentation (HTML)](/Submission/DataStructures_Documentation.html)** - Comprehensive technical documentation of all implemented data structures

---

## About

A comprehensive Windows Forms desktop application for South African municipalities, enabling citizens to report issues, access local events, and track service requests. Built with C# .NET Framework 4.7.2 and SQLite database.

---

## System Requirements

### Minimum Requirements
- Windows 7 SP1 or later
- .NET Framework 4.7.2 or later
- 512 MB RAM
- 100 MB available disk space

### Recommended Requirements
- Windows 10 or Windows 11
- .NET Framework 4.8
- 2 GB RAM
- Visual Studio 2019 or later (for building from source)

---

## Installation

### Prerequisites

Before building or running the application, ensure you have:

1. **.NET Framework 4.7.2 or later**
   - Download from: https://dotnet.microsoft.com/download/dotnet-framework/net472

2. **Visual Studio 2019 or later** (for building from source)
   - Required workload: .NET desktop development
   - Download from: https://visualstudio.microsoft.com/

3. **Git** (for cloning the repository)
   - Download from: https://git-scm.com/

---

## Building the Project

### Step 1: Clone the Repository

```powershell
git clone https://github.com/Max0xDay/MunicipalApp.git
cd MunicipalApp
```

### Step 2: Open in Visual Studio

1. Open `Sidequest municiple app.sln` in Visual Studio
2. Wait for NuGet packages to restore automatically

### Step 3: Build the Solution

**Using Visual Studio:**
- Press `Ctrl+Shift+B`, or
- Select `Build > Build Solution` from the menu, or
- Right-click the solution in Solution Explorer and select `Build Solution`

**Using Command Line:**

```powershell
# Using provided build script
.\build.bat

# Or using MSBuild directly
msbuild "Sidequest municiple app.sln" /p:Configuration=Debug
```


## Running the Application

### Option 1: Run Pre-Built Executable

1. Navigate to the release directory:
   ```powershell
   cd "bin\Debug"
   ```

2. Run the executable:
   ```powershell
   .\Sidequest municiple app.exe
   ```

3. The application will automatically create the SQLite database on first launch

### Option 2: Run from Visual Studio

**With Debugging:**
- Press `F5`

**Without Debugging:**
- Press `Ctrl+F5`

---

## Project Structure

```
Sidequest municiple app/
├── Forms/                          # UI Layer
│   ├── MainMenuForm.cs             # Main navigation menu
│   ├── ReportIssuesForm.cs         # Issue reporting interface
│   ├── LocalEventsForm.cs          # Events and announcements
│   └── ServiceRequestStatusForm.cs # Request tracking
├── Models/                         # Data Models
│   ├── Issue.cs                    # Issue entity
│   ├── LocalEvent.cs               # Event entity
│   └── ServiceRequest.cs           # Service request entity
├── DataAccess/                     # Database Layer
│   └── DatabaseHelper.cs           # SQLite operations
├── DataStructures/                 # Custom Data Structure Implementations
│   ├── ServiceRequestTree.cs       # Basic tree
│   ├── ServiceRequestBinaryTree.cs # Binary tree
│   ├── ServiceRequestBST.cs        # Binary search tree
│   ├── ServiceRequestAVL.cs        # Self-balancing AVL tree
│   ├── ServiceRequestRedBlackTree.cs # Red-black tree
│   ├── ServiceRequestHeap.cs       # Min/Max heap
│   ├── ServiceRequestGraph.cs      # Graph with BFS/DFS/MST
│   ├── PriorityCategoryQueue.cs    # Priority queue
│   ├── IssuePredictionTrie.cs      # Trie for predictions
│   └── Node Classes/               # Supporting node structures
├── Utilities/                      # Helper Classes
│   ├── AppPalette.cs               # UI color scheme
│   ├── DataSeeder.cs               # Test data generation
│   └── SpellChecker.cs             # Spell checking utility
├── Properties/                     # Assembly configuration
├── Submission/                     # Submission documents
│   ├── Max Day ST10322238 Final PROG7312 Submission Document.pdf
│   └── DataStructures_Documentation.html (copy)
└── DataStructures_Documentation.html # Technical documentation
```

---

## Technologies Used

- **Framework:** .NET Framework 4.7.2
- **Language:** C# 7.3
- **UI Framework:** Windows Forms
- **Database:** SQLite 3.x (embedded)
- **IDE:** Visual Studio 2019/2022
- **Build System:** MSBuild
- **Version Control:** Git

### NuGet Packages
- System.Data.SQLite.Core (v1.0.118.0)
- System.Data.SQLite (v2.0.1)
- Stub.System.Data.SQLite.Core.NetFramework (v1.0.118.0)

---
