# Municipal Services Application

**PROG7312 Portfolio of Evidence - Advanced Application Development**

**Author:** Max Day

**Submission Repository:** https://github.com/VCSTDN2024/prog7312-poe-ML0Day0VC  
**Development Repository:** https://github.com/Max0xDay/MunicipalApp

---

## Important Notes

⚠️ **Complete Data Structures Documentation:** All data structures are comprehensively documented in `DataStructures_Documentation.html`. Please review this file for detailed implementation explanations.

📊 **GitHub Repositories:** Development history is in the personal repository (Max0xDay/MunicipalApp). The submission repository (VCSTDN2024/prog7312-poe-ML0Day0VC) was provided later in the project timeline.




---

## Table of Contents

- [Project Overview](#project-overview)
- [Quick Start](#quick-start)
- [Application Features](#application-features)
- [Data Structures Implementation](#data-structures-implementation)
- [Project Structure](#project-structure)
- [Changelog](#changelog)

---

## Project Overview

A comprehensive Windows Forms desktop application for South African municipalities, enabling citizens to report issues, access local events, and track service requests. Built with C# .NET Framework 4.7.2 and SQLite database.

### Three-Part Implementation

**Part 1: Report Issues** ✅
- Issue reporting with location, category, and description
- Media attachment support (images/documents)
- Progress bar user engagement strategy
- SQLite database persistence

**Part 2: Local Events and Announcements** ✅
- Event browsing with advanced search capabilities
- Multi-criteria filtering (category + date)
- Recommendation engine based on user patterns
- Sorted dictionaries, priority queues, and hash sets

**Part 3: Service Request Status** ✅
- Real-time service request tracking with unique IDs
- Advanced data structures: BST, AVL, Red-Black Trees, Heaps, Graphs
- Graph algorithms: BFS, DFS, Minimum Spanning Tree
- Trie-based issue prediction and search

---

## Quick Start

### System Requirements
- Windows 7 SP1 or later (Windows 10/11 recommended)
- .NET Framework 4.7.2 or later
- 100 MB disk space

### Running the Application

**Option 1: Pre-Built Executable**
```powershell
cd "bin\Debug"
.\Sidequest municiple app.exe
```

**Option 2: Build from Source**
```powershell
# Clone repository
git clone https://github.com/Max0xDay/MunicipalApp.git
cd MunicipalApp

# Open in Visual Studio
start "Sidequest municiple app.sln"

# Or build with command line
.\build.bat
```

### First-Time Setup
1. Launch the application
2. Click **"Generate Test Data"** button on main menu
3. Choose 25 records (quick) or 100 records (comprehensive)
4. Explore all three features with populated data

---

## Application Features

### 1. Report Issues
**How to Use:**
1. Click "Report Issues" from main menu
2. Enter location, select category, write description
3. Optionally attach media files (images/documents)
4. Monitor progress bar showing completion
5. Submit to receive unique reference ID

**Key Features:**
- Categories: Sanitation, Roads, Utilities, Water, Electricity
- File attachments: JPG, PNG, PDF, DOCX
- Real-time progress tracking
- Database persistence

### 2. Local Events and Announcements
**How to Use:**
1. Click "Local Events and Announcements"
2. Browse upcoming events in chronological order
3. Use search box to filter by keywords
4. Filter by category using dropdown
5. Select date range for temporal filtering
6. View personalized recommendations based on your searches

**Key Features:**
- Sorted dictionary for efficient event organization
- Priority queue for upcoming events
- Hash set for unique categories
- Intelligent recommendation system
- Multi-criteria search

### 3. Service Request Status
**How to Use:**
1. Click "Service Request Status"
2. View all requests in main data grid
3. Search by unique request ID
4. Filter by status, category, or priority
5. Explore different data structure visualizations
6. View related requests in graph visualization

**Key Features:**
- Binary Search Tree for O(log n) searches
- AVL and Red-Black Trees for balanced operations
- Min-heap for priority queue management
- Graph with BFS/DFS traversal
- Minimum Spanning Tree for related issues
- Trie for autocomplete predictions

---

## Data Structures Implementation

### Trees (Part 3 Requirement)
| Structure | Purpose | Time Complexity |
|-----------|---------|-----------------|
| **Binary Tree** | Hierarchical organization | O(n) search |
| **Binary Search Tree** | Sorted data storage | O(log n) avg search |
| **AVL Tree** | Self-balancing BST | O(log n) guaranteed |
| **Red-Black Tree** | Efficient insertions | O(log n) operations |

### Heaps (Part 3 Requirement)
| Structure | Purpose | Use Case |
|-----------|---------|----------|
| **Min-Heap** | Priority queue | High-priority requests first |
| **Max-Heap** | Priority queue | Most recent requests first |

### Graphs (Part 3 Requirement)
| Feature | Algorithm | Purpose |
|---------|-----------|---------|
| **Graph Structure** | Adjacency List | Related issue clustering |
| **BFS Traversal** | Breadth-First Search | Level-wise exploration |
| **DFS Traversal** | Depth-First Search | Deep relationship discovery |
| **Minimum Spanning Tree** | Prim's Algorithm | Optimal issue connections |

### Advanced Structures (Part 2 & 3)
- **Sorted Dictionary**: Event organization by date (O(log n))
- **Hash Set**: Unique category tracking (O(1))
- **Priority Queue**: Event prioritization
- **Trie**: Issue prediction and autocomplete
- **Custom Queue**: Search history for recommendations

📖 **Detailed Documentation:** See `DataStructures_Documentation.html` for comprehensive explanations with code examples.

---

## Project Structure

```
Sidequest municiple app/
├── Forms/                      # UI Layer
│   ├── MainMenuForm.cs         # Main navigation
│   ├── ReportIssuesForm.cs     # Issue reporting
│   ├── LocalEventsForm.cs      # Events and search
│   └── ServiceRequestStatusForm.cs # Request tracking
├── Models/                     # Data Models
│   ├── Issue.cs
│   ├── LocalEvent.cs
│   └── ServiceRequest.cs
├── DataAccess/                 # Database Layer
│   └── DatabaseHelper.cs       # SQLite operations
├── DataStructures/             # Custom Implementations
│   ├── ServiceRequestBST.cs
│   ├── ServiceRequestAVL.cs
│   ├── ServiceRequestRedBlackTree.cs
│   ├── ServiceRequestHeap.cs
│   ├── ServiceRequestGraph.cs
│   └── IssuePredictionTrie.cs
└── Utilities/
    ├── AppPalette.cs          # UI theming
    ├── DataSeeder.cs          # Test data generation
    └── SpellChecker.cs
```

---

## Changelog

### Part 3 Updates (Complete Implementation)
**Added:**
- Service Request Status feature with complete data structure suite
- Binary Search Tree, AVL Tree, Red-Black Tree implementations
- Min/Max heap priority queue system
- Graph with BFS, DFS, and MST algorithms
- Trie-based issue prediction system
- Related issue clustering and visualization
- Advanced filtering (status, category, priority)
- Unique ID search functionality

**Improved:**
- Database schema with service request relationships
- Test data generation (now includes 50 related issue clusters)
- Main menu now enables all three features
- Consistent UI theming across all forms
- Error handling and user feedback

### Part 2 Updates (Local Events Implementation)
**Added:**
- Local Events and Announcements feature
- Event search with category and date filtering
- Recommendation engine based on user patterns
- Sorted dictionary for event organization
- Priority queue for upcoming events
- Hash set for unique categories

**Improved:**
- Database integration for event persistence
- Search algorithm efficiency
- UI consistency with Part 1

### Part 1 (Initial Release)
**Implemented:**
- Report Issues feature with full functionality
- SQLite database integration
- Progress bar user engagement strategy
- Media attachment support
- Input validation
- Admin dashboard for viewing reported issues

---

## Contact & Support

**Developer:** Max Day  
**GitHub:** https://github.com/Max0xDay  
**Project Repository:** https://github.com/Max0xDay/MunicipalApp

For questions or issues, please open an issue on the GitHub repository.

---

*This application was developed as part of PROG7312 - Advanced Application Development at The Independent Institute of Education.*
