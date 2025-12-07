# High-Availability Student Management System

A mission-critical Student Management System with enterprise-grade reliability, implementing advanced SQL Server features and dual business logic architectures.

## 👥 Team Members
- **Mustufa** - 26100038
- **Eman Nabeel** - 26100270
- **Dayyan Ali Akhtar** - 26100007
- **Shaheer Ahmad** - 26100279

---

## 📋 Table of Contents
1. [Project Overview](#project-overview)
2. [System Architecture](#system-architecture)
3. [Database Features](#database-features)
4. [Prerequisites](#prerequisites)
5. [Installation & Setup](#installation--setup)
6. [Running the Application](#running-the-application)
7. [Using the Application](#using-the-application)
8. [Project Structure](#project-structure)
9. [Configuration](#configuration)
10. [Troubleshooting](#troubleshooting)

---

## 🎯 Project Overview

This system demonstrates a production-ready student management application with:
- **3-Tier Architecture**: Data Access Layer (DAL), Business Logic Layer (BLL), User Interface (UI)
- **Dual BLL Implementation**: LINQ/Entity Framework + Stored Procedures (switchable at runtime via Factory Pattern)
- **Advanced SQL Features**: Stored Procedures, Functions, Triggers, Views, CTEs, Indexes, Table Partitioning
- **100,000+ Students**: Realistic data scale with 900,000+ enrollments across 500 courses
- **Enterprise Data Integrity**: Automated auditing, soft deletes, prerequisite validation, capacity management

### Key Features
✅ **Intelligent Course Registration** - Multi-level validation (holds, prerequisites, capacity)  
✅ **Automated Grade Auditing** - Immutable audit trail via database triggers  
✅ **Real-Time GPA Calculation** - Weighted GPA with database functions  
✅ **Department Hierarchy** - Recursive CTE for organizational reporting  
✅ **Dual Business Logic** - Runtime switching between LINQ and Stored Procedures  

---

## 🏗️ System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    USER INTERFACE (UI)                      │
│              WinForms Application (.NET 4.8)                │
│  MainForm | StudentForm | CourseForm | EnrollmentForm      │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│              BUSINESS LOGIC LAYER (BLL)                     │
│                   Factory Pattern                           │
├─────────────────────┬───────────────────────────────────────┤
│  LINQ Implementation │  Stored Procedure Implementation     │
│  - Entity Framework  │  - ADO.NET SqlCommand                │
│  - LINQ Queries      │  - Direct SP Calls                   │
└─────────────────────┴───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│              DATA ACCESS LAYER (DAL)                        │
│         Entity Framework 6.4.4 DbContext                    │
│    9 POCO Models with Navigation Properties                │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│                 SQL SERVER DATABASE                         │
│   9 Tables | 3 SPs | 3 Functions | 2 Triggers | 3 Views    │
│   Partitioning | Indexes | CTEs | Constraints               │
└─────────────────────────────────────────────────────────────┘
```

### Project Structure
```
StudentManagementSystem/
│
├── StudentManagementSystem.DAL/          # Data Access Layer
│   ├── Models/                           # Entity classes (Student, Course, etc.)
│   ├── StudentManagementContext.cs       # EF DbContext
│   └── App.config                        # Connection string
│
├── StudentManagementSystem.BLL/          # Business Logic Layer
│   ├── Interfaces/                       # Service contracts
│   │   ├── IStudentService.cs
│   │   ├── ICourseService.cs
│   │   ├── IDepartmentService.cs
│   │   ├── IEnrollmentService.cs
│   │   └── IBusinessLogicFactory.cs
│   │
│   ├── Factory/                          # Factory Pattern implementation
│   │   └── BusinessLogicFactory.cs       # Creates LINQ or SP services
│   │
│   ├── LinqImplementation/               # Entity Framework implementation
│   │   ├── LinqStudentService.cs
│   │   ├── LinqCourseService.cs
│   │   ├── LinqDepartmentService.cs
│   │   └── LinqEnrollmentService.cs
│   │
│   └── StoredProcedureImplementation/    # ADO.NET + SP implementation
│       ├── SPStudentService.cs
│       ├── SPCourseService.cs
│       ├── SPDepartmentService.cs
│       └── SPEnrollmentService.cs
│
└── StudentManagementSystem.UI/           # User Interface Layer
    ├── Forms/                            # WinForms
    │   ├── MainForm.cs                   # Navigation hub + BLL switcher
    │   ├── StudentForm.cs                # Student CRUD
    │   ├── CourseForm.cs                 # Course CRUD
    │   ├── EnrollmentForm.cs             # Registration + Grade management
    │   └── DepartmentForm.cs             # Department hierarchy
    │
    └── Program.cs                        # Application entry point
```

---

## 🗄️ Database Features

### Stored Procedures (3)
| Name | Purpose | Key Logic |
|------|---------|-----------|
| `sp_RegisterStudentForCourse` | Course registration | Checks holds → validates prerequisites → verifies capacity → atomic transaction |
| `sp_GetDepartmentHierarchy` | Organizational structure | Recursive CTE builds dept tree with Level/HierarchyPath |
| `sp_GetStudentTranscriptAndGPA` | Academic records | Calls TVF + scalar function for transcript + GPA |

### Functions (3)
| Name | Type | Purpose |
|------|------|---------|
| `fn_CheckPrerequisitesMet` | Scalar | CTE-based validation: checks if student passed all prereqs with A-D |
| `fn_CalculateGPA` | Scalar | CTE with weighted calculation: SUM(GradePoints × Credits) / SUM(Credits) |
| `fn_GetStudentTranscript` | Table-Valued | 5-table JOIN returning formatted course history |

### Triggers (2)
| Name | Type | Purpose |
|------|------|---------|
| `trg_After_GradeUpdate` | AFTER UPDATE | Auto-populates `Audit_GradeChanges` table with old/new grades, admin ID, timestamp |
| `trg_InsteadOf_DeleteStudent` | INSTEAD OF DELETE | Soft delete: sets `EnrollmentStatus='Inactive'` instead of removing row |

### Views (3)
| Name | Purpose |
|------|---------|
| `vw_CourseCatalog` | Active courses with department info (filters IsActive departments) |
| `vw_AvailableCourseOfferings` | Open sections with computed `SeatsRemaining` column |
| `vw_StudentTranscript` | Complete transcript with grades, semesters (5-table JOIN) |

### Indexes (7+)
- **Filtered**: `IX_Students_Active` (WHERE EnrollmentStatus='Active'), `IX_Enrollments_Failing` (WHERE Grade='F')
- **Unique**: `UQ_Semesters_Year_Season`, `UQ_CourseOfferings_Course_Semester`, `UQ_Enrollments_Student_Offering`
- **FK Helpers**: `IX_Enrollments_StudentID`, `IX_Enrollments_OfferingID`, `IX_Courses_DepartmentID`, `IX_StudentHolds_Student_IsActive`

### Table Partitioning (2)
- **Enrollments**: Partitioned by `EnrollmentDate` (yearly: 2022-2026) using `PF_EnrollmentsByDate`
- **Audit_GradeChanges**: Partitioned by `EnrollmentDate` (yearly: 2022-2026) using `PF_AuditByDate`

### Data Scale
- **100,002 Students** | **500 Courses** | **50 Departments** | **7,500 Course Offerings**
- **900,000+ Enrollments** | **5,000+ Student Holds** | **20,000+ Grade Audit Records**
- **Total: 1,033,000+ rows** across all tables

---

## 🔧 Prerequisites

### Required Software
1. **SQL Server 2019+** (Express, Developer, or Enterprise)
   - Download: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - SQL Server Management Studio (SSMS) recommended

2. **Visual Studio 2019+** or **MSBuild Tools**
   - Visual Studio Community 2022 (recommended): https://visualstudio.microsoft.com/downloads/
   - OR MSBuild Tools: https://aka.ms/vs/17/release/vs_BuildTools.exe

3. **.NET Framework 4.8**
   - Usually included with Windows 10/11
   - Download if needed: https://dotnet.microsoft.com/download/dotnet-framework/net48

4. **Entity Framework 6.4.4**
   - Included via NuGet (restored automatically on build)

### Verify Prerequisites
```powershell
# Check SQL Server
Get-Service | Where-Object {$_.Name -like "*SQL*"}

# Check .NET Framework
Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\' | Get-ItemProperty -Name Version

# Check Visual Studio or MSBuild
Get-ChildItem "C:\Program Files*" -Recurse -Filter "MSBuild.exe" -ErrorAction SilentlyContinue | Select-Object FullName
```

---

## 🚀 Installation & Setup

### Step 1: Clone the Repository
```bash
git clone https://github.com/Shaheer-Ahmd/ha-sms.git
cd ha-sms
```

### Step 2: Create the Database
1. Open **SQL Server Management Studio (SSMS)**
2. Connect to your SQL Server instance
3. Open the file: `StudentManagementDB_creation.sql`
4. **Execute the entire script** (This will take 2-3 minutes):
   - Drops and recreates `StudentManagementDB`
   - Creates all tables with partitioning
   - Creates indexes, views, functions, stored procedures, triggers
   - Populates with 100K+ students and realistic data

**Verify Database Creation:**
```sql
USE StudentManagementDB;

-- Check row counts
SELECT 'Students' AS TableName, COUNT(*) AS RowCount FROM Students
UNION ALL
SELECT 'Enrollments', COUNT(*) FROM Enrollments
UNION ALL
SELECT 'Courses', COUNT(*) FROM Courses;

-- Should show ~100K students, ~900K enrollments, ~500 courses
```

### Step 3: Configure Connection String
Edit `StudentManagementSystem.DAL\App.config` and `StudentManagementSystem.UI\App.config`:

```xml
<connectionStrings>
  <add name="StudentManagementDB" 
       connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=StudentManagementDB;Integrated Security=True;MultipleActiveResultSets=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Replace `YOUR_SERVER_NAME` with your SQL Server instance name:**
- Local default instance: `localhost` or `.` or `(local)`
- Named instance: `localhost\SQLEXPRESS` or `.\SQLEXPRESS`
- Remote server: `SERVER_NAME\INSTANCE` or `IP_ADDRESS`

**To find your server name:**
```sql
-- Run in SSMS
SELECT @@SERVERNAME;
```

### Step 4: Restore NuGet Packages
Open PowerShell in the project root:
```powershell
# Option 1: Using nuget.exe (already in repo)
.\nuget.exe restore StudentManagementSystem.sln

# Option 2: Using MSBuild
$msbuild = Get-ChildItem "C:\Program Files*" -Recurse -Filter "MSBuild.exe" -ErrorAction SilentlyContinue | 
           Where-Object { $_.FullName -like "*Current*Bin*" } | 
           Select-Object -First 1 -ExpandProperty FullName
& $msbuild StudentManagementSystem.sln /t:Restore
```

### Step 5: Build the Solution
```powershell
# Find MSBuild
$msbuild = Get-ChildItem "C:\Program Files*" -Recurse -Filter "MSBuild.exe" -ErrorAction SilentlyContinue | 
           Where-Object { $_.FullName -like "*Current*Bin*" } | 
           Select-Object -First 1 -ExpandProperty FullName

# Build the solution
& $msbuild StudentManagementSystem.sln /t:Build /p:Configuration=Debug /v:minimal
```

**Expected Output:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

---

## ▶️ Running the Application

### Method 1: Using PowerShell (Recommended)
```powershell
# Navigate to project directory
cd C:\path\to\ha-sms

# Run the application
Start-Process "StudentManagementSystem.UI\bin\Debug\StudentManagementSystem.UI.exe"
```

### Method 2: Using Visual Studio
1. Open `StudentManagementSystem.sln` in Visual Studio
2. Right-click on `StudentManagementSystem.UI` project
3. Select **"Set as StartUp Project"**
4. Press **F5** or click **Start**

### Method 3: Direct Executable
Navigate to: `StudentManagementSystem.UI\bin\Debug\`  
Double-click: `StudentManagementSystem.UI.exe`

---

## 📖 Using the Application

### Main Form - Navigation Hub
![Main Form](docs/mainform.png)

**BLL Implementation Switcher:**
- Current implementation displayed in status label
- Click **"Switch BLL Implementation"** to toggle between:
  - **LINQ**: Entity Framework with LINQ queries
  - **Stored Procedure**: Direct ADO.NET calls to SPs

**Navigation Buttons:**
- **Manage Students** → Student CRUD form
- **Manage Courses** → Course CRUD form
- **Manage Enrollments** → Registration and grade management
- **Manage Departments** → Department hierarchy

---

### Student Management Form

**Features:**
- ✅ **View All Students** - Grid with StudentID, Name, Email, Status, DOB
- ✅ **Add Student** - Enter details and click "Add"
- ✅ **Update Student** - Select from grid, modify fields, click "Update"
- ✅ **Delete Student** - Select and click "Delete" (soft delete to 'Inactive')
- ✅ **Search** - Filter by name or email
- ✅ **View Transcript** - Shows completed courses with grades
- ✅ **Calculate GPA** - Real-time weighted GPA calculation

**Workflow:**
1. Grid auto-loads all students on form open
2. Click a row to populate detail fields
3. Modify fields and click "Update" to save changes
4. Click "Add" (without selecting a row) to create new student
5. Click "View Transcript" to see academic history
6. Click "Calculate GPA" to see current GPA

**Important Notes:**
- Email must be unique (enforced by database)
- EnrollmentStatus: Active, Inactive, Graduated, Suspended
- Delete operation sets status to 'Inactive' (preserves history)

---

### Course Management Form

**Features:**
- ✅ **View All Courses** - Grid with CourseID, Code, Title, Credits, Department
- ✅ **Add Course** - Create new course with prerequisites
- ✅ **Update Course** - Modify existing course details
- ✅ **Delete Course** - Remove course (if no enrollments)
- ✅ **Check Prerequisites** - Validate if student meets prereqs

**Workflow:**
1. Select department from dropdown
2. Enter course code (must be unique, e.g., "CS101")
3. Enter title, description, credits (1-6)
4. Click "Add" to create course
5. To update: select from grid, modify, click "Update"

**Important Notes:**
- CourseCode must be unique across all departments
- Credits constrained between 1 and 6
- Cannot delete courses with active enrollments

---

### Enrollment Management Form

**Features:**
- ✅ **Register Students** - Uses `sp_RegisterStudentForCourse` with validation
- ✅ **View Enrollments** - See student's current and past enrollments
- ✅ **Update Grades** - Triggers `trg_After_GradeUpdate` for audit trail
- ✅ **Available Offerings** - Shows open sections (uses `vw_AvailableCourseOfferings`)

**Workflow - Registering a Student:**
1. Select student from dropdown
2. Select semester
3. Grid shows available course offerings with seats remaining
4. Select offering row
5. Click "Register"
6. System validates:
   - ✅ No active student holds (Financial, Academic, Disciplinary)
   - ✅ All prerequisites met with passing grades (A-D)
   - ✅ Course has available capacity
7. If validation passes → student enrolled, capacity decremented
8. If validation fails → error message explains which check failed

**Workflow - Updating Grades:**
1. Select student from dropdown
2. Grid shows current enrollments
3. Select enrollment row
4. Select grade (A, B, C, D, F)
5. Click "Update Grade"
6. **Audit trigger fires automatically** → records change in `Audit_GradeChanges` table

**Important Notes:**
- Registration uses stored procedure with atomic transaction
- Hold check prevents registration if any active holds exist
- Prerequisite validation uses `fn_CheckPrerequisitesMet` function
- Grade changes are immutably logged (OldGrade, NewGrade, AdminID, Timestamp)

---

### Department Management Form

**Features:**
- ✅ **View Departments** - Grid with hierarchy info
- ✅ **Add Department** - Create new department (can specify parent)
- ✅ **Update Department** - Modify name, parent, active status
- ✅ **Delete Department** - Remove department (if no courses)
- ✅ **View Hierarchy** - Shows recursive organizational tree

**Workflow - View Hierarchy:**
1. Click "View Hierarchy" button
2. System calls `sp_GetDepartmentHierarchy`
3. **Recursive CTE executes** → builds tree with Level and HierarchyPath
4. New window displays indented department structure:
   ```
   Engineering (Level 0)
     → Computer Science (Level 1)
       → Artificial Intelligence (Level 2)
     → Electrical Engineering (Level 1)
   ```

**Important Notes:**
- ParentDepartmentID creates hierarchical relationships
- Recursive CTE handles unlimited depth
- Cannot delete departments with sub-departments or courses

---

## ⚙️ Configuration

### App.config Settings

#### Connection String
```xml
<connectionStrings>
  <add name="StudentManagementDB" 
       connectionString="Data Source=localhost;Initial Catalog=StudentManagementDB;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

#### BLL Implementation Selector
```xml
<appSettings>
  <!-- Options: "LINQ" or "StoredProcedure" -->
  <add key="BLLImplementation" value="LINQ"/>
</appSettings>
```

**Change at Runtime:**
- Click "Switch BLL Implementation" button on MainForm
- Changes take effect immediately for all subsequent operations
- No need to restart application

---

## 🐛 Troubleshooting

### Database Connection Errors

**Error:** "Cannot open database 'StudentManagementDB'"
```powershell
# Solution 1: Verify database exists
# Run in SSMS:
SELECT name FROM sys.databases WHERE name = 'StudentManagementDB';

# Solution 2: Re-run database creation script
# Execute: StudentManagementDB_creation.sql
```

**Error:** "Login failed for user"
```xml
<!-- Solution: Add explicit credentials to connection string -->
<connectionStrings>
  <add name="StudentManagementDB" 
       connectionString="Data Source=localhost;Initial Catalog=StudentManagementDB;User Id=sa;Password=YourPassword"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Build Errors

**Error:** "Could not find MSBuild.exe"
```powershell
# Install Visual Studio Build Tools
# Download: https://aka.ms/vs/17/release/vs_BuildTools.exe
# OR find existing MSBuild:
Get-ChildItem "C:\Program Files*" -Recurse -Filter "MSBuild.exe" | Select-Object FullName
```

**Error:** "NuGet packages missing"
```powershell
# Restore packages
.\nuget.exe restore StudentManagementSystem.sln
```

**Error:** "Entity Framework not found"
```powershell
# Install EF 6.4.4
Install-Package EntityFramework -Version 6.4.4 -ProjectName StudentManagementSystem.DAL
```

### Runtime Errors

**Error:** "UPDATE failed because SET options have incorrect settings"
- **Cause:** This was an indexed view conflict (already fixed in codebase)
- **Solution:** Update to latest code - delete operations now use direct SqlConnection

**Error:** "Registration failed: Student has active holds"
- **Cause:** Student has financial, academic, or disciplinary hold
- **Solution:** Query `StudentHolds` table and set `IsActive=0` for testing:
  ```sql
  UPDATE StudentHolds SET IsActive = 0 WHERE StudentID = 12345;
  ```

**Error:** "Registration failed: Prerequisites not met"
- **Cause:** Student hasn't passed required prerequisite courses
- **Solution:** Either:
  1. Enroll in prerequisite courses first
  2. Insert passing grades for prerequisites (for testing):
     ```sql
     -- Find prerequisite courses
     SELECT * FROM CoursePrerequisites WHERE CourseID = 123;
     
     -- Add passing enrollment
     INSERT INTO Enrollments (StudentID, OfferingID, Grade, EnrollmentDate)
     VALUES (12345, [OfferingID of Prereq Course], 'B', GETDATE());
     ```

**Error:** "Registration failed: Course is at maximum capacity"
- **Solution:** Increase MaxCapacity or drop another student:
  ```sql
  UPDATE CourseOfferings SET MaxCapacity = MaxCapacity + 10 WHERE OfferingID = 456;
  ```

---

## 📊 Testing the System

### Verify SQL Features

#### Test Stored Procedures
```sql
-- Test 1: sp_RegisterStudentForCourse
EXEC sp_RegisterStudentForCourse @StudentID = 1, @OfferingID = 1;

-- Test 2: sp_GetDepartmentHierarchy
EXEC sp_GetDepartmentHierarchy;

-- Test 3: sp_GetStudentTranscriptAndGPA
EXEC sp_GetStudentTranscriptAndGPA @StudentID = 1;
```

#### Test Functions
```sql
-- Test 1: fn_CheckPrerequisitesMet
SELECT dbo.fn_CheckPrerequisitesMet(1, 100) AS PrerequisitesMet;

-- Test 2: fn_CalculateGPA
SELECT dbo.fn_CalculateGPA(1) AS GPA;

-- Test 3: fn_GetStudentTranscript (TVF)
SELECT * FROM dbo.fn_GetStudentTranscript(1);
```

#### Test Triggers
```sql
-- Test 1: trg_After_GradeUpdate
UPDATE Enrollments SET Grade = 'A' WHERE EnrollmentID = 1;
SELECT * FROM Audit_GradeChanges WHERE EnrollmentID = 1;

-- Test 2: trg_InsteadOf_DeleteStudent
DELETE FROM Students WHERE StudentID = 99999;
SELECT EnrollmentStatus FROM Students WHERE StudentID = 99999; -- Should be 'Inactive'
```

#### Test Views
```sql
-- Test 1: vw_CourseCatalog
SELECT TOP 10 * FROM vw_CourseCatalog ORDER BY CourseCode;

-- Test 2: vw_AvailableCourseOfferings
SELECT TOP 10 * FROM vw_AvailableCourseOfferings WHERE SeatsRemaining > 0;

-- Test 3: vw_StudentTranscript
SELECT * FROM vw_StudentTranscript WHERE StudentID = 1;
```

#### Test Partitioning
```sql
-- Verify Enrollments partitioning
SELECT 
    $PARTITION.PF_EnrollmentsByDate(EnrollmentDate) AS PartitionNumber,
    MIN(EnrollmentDate) AS MinDate,
    MAX(EnrollmentDate) AS MaxDate,
    COUNT(*) AS RowCount
FROM Enrollments
GROUP BY $PARTITION.PF_EnrollmentsByDate(EnrollmentDate)
ORDER BY PartitionNumber;

-- Verify Audit_GradeChanges partitioning
SELECT 
    $PARTITION.PF_AuditByDate(EnrollmentDate) AS PartitionNumber,
    COUNT(*) AS RowCount
FROM Audit_GradeChanges
GROUP BY $PARTITION.PF_AuditByDate(EnrollmentDate)
ORDER BY PartitionNumber;
```

---

## 📁 Deleting the Tests Folder

**Yes, you can safely delete `StudentManagementSystem.Tests/`**

The test folder is **not referenced** in the solution file and was created only for development testing. It has no impact on application functionality.

```powershell
# Delete the test folder
Remove-Item -Path "StudentManagementSystem.Tests" -Recurse -Force
```

After deletion, your clean structure will be:
```
ha-sms/
├── StudentManagementSystem.DAL/
├── StudentManagementSystem.BLL/
├── StudentManagementSystem.UI/
├── StudentManagementDB_creation.sql
└── StudentManagementSystem.sln
```

---

## 🔄 Factory Pattern Explanation

### How It Works

The application uses the **Factory Design Pattern** to switch between two BLL implementations at runtime:

```csharp
// Factory creates appropriate services based on configuration
public class BusinessLogicFactory : IBusinessLogicFactory
{
    private BLLImplementationType _implementationType;
    
    public IStudentService GetStudentService()
    {
        switch (_implementationType)
        {
            case BLLImplementationType.LINQ:
                return new LinqStudentService(_connectionString);
            case BLLImplementationType.StoredProcedure:
                return new SPStudentService(_connectionString);
        }
    }
}
```

### Benefits
1. **Flexibility**: Switch data access strategies without changing UI code
2. **Testability**: Compare performance of LINQ vs SP implementations
3. **Maintainability**: Both implementations use same interface contracts
4. **Performance Analysis**: Benchmark Entity Framework vs raw ADO.NET

### Usage in Forms
```csharp
public class StudentForm : Form
{
    private IBusinessLogicFactory _bllFactory;
    private IStudentService _studentService;
    
    public StudentForm(IBusinessLogicFactory bllFactory)
    {
        _bllFactory = bllFactory;
        _studentService = _bllFactory.GetStudentService(); // Gets LINQ or SP version
    }
}
```

---

## 📚 Additional Resources

### Entity Framework 6.4.4
- Documentation: https://docs.microsoft.com/en-us/ef/ef6/

### SQL Server Features
- Stored Procedures: https://docs.microsoft.com/en-us/sql/relational-databases/stored-procedures/
- Functions: https://docs.microsoft.com/en-us/sql/t-sql/functions/
- Triggers: https://docs.microsoft.com/en-us/sql/relational-databases/triggers/
- Table Partitioning: https://docs.microsoft.com/en-us/sql/relational-databases/partitions/

### Design Patterns
- Factory Pattern: https://refactoring.guru/design-patterns/factory-method

---

## 🤝 Contributing

This is an academic project for database systems coursework. For questions or issues, contact team members.

---

## 📄 License

This project is submitted as part of university coursework. All rights reserved to the team members listed above.

---

## ✅ Phase Completion Checklist

- [x] **Phase 1**: SQL Database with 3 SPs, 3 Functions, 2 Triggers, 3 Views, CTEs, Indexes, Partitioning
- [x] **Phase 2**: .NET Application with Factory Pattern, Dual BLL (LINQ + SP), 4 CRUD Forms
- [x] **Phase 3**: Application packaging, Documentation, SQL Feature Summary
- [ ] **Extra Credit**: Kubernetes High Availability (Optional)

---

## 📞 Support

For technical issues or questions:
- Check [Troubleshooting](#troubleshooting) section
- Review SQL script comments in `StudentManagementDB_creation.sql`
- Examine BLL service implementations for business logic details

**Last Updated:** December 7, 2025  
**Version:** 1.0 (Phase 3 Submission)