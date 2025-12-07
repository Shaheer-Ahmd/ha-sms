# Student Management System - Phase 2 Application

## Overview
This is a complete .NET Framework 4.8 WinForms application that interacts with the SQL Server Student Management System database created in Phase 1. The application demonstrates the Factory Design Pattern with two interchangeable Business Logic Layer (BLL) implementations: LINQ/Entity Framework and Stored Procedures.

## Architecture

### 1. Data Access Layer (DAL)
- **StudentManagementSystem.DAL** - Entity Framework 6.4.4
- All database entities mapped to POCO classes
- `StudentManagementContext` - DbContext with proper relationships
- Models: Student, Course, Department, Semester, Enrollment, CourseOffering, StudentHold, AuditGradeChange, StudentTranscript

### 2. Business Logic Layer (BLL)
- **StudentManagementSystem.BLL** - Service layer with two implementations

#### Interfaces
- `IStudentService`
- `ICourseService`
- `IDepartmentService`
- `ISemesterService`
- `IEnrollmentService`
- `IStudentHoldService`
- `ICourseOfferingService`
- `IBusinessLogicFactory` - Factory interface

#### LINQ Implementation (`LinqImplementation/`)
- Uses Entity Framework and LINQ queries
- Leverages navigation properties and eager loading
- Demonstrates use of:
  - Views (`vw_StudentTranscript`, `vw_CourseCatalog`)
  - Filtered indexes (Active students, failing students)
  - Table partitioning (automatically handled by EF)

#### Stored Procedure Implementation (`StoredProcedureImplementation/`)
- Uses raw ADO.NET with SqlConnection/SqlCommand
- Calls database stored procedures directly
- Demonstrates use of:
  - `sp_RegisterStudentForCourse` - Checks holds, prerequisites, capacity
  - `sp_GetStudentTranscriptAndGPA` - Uses TVF `fn_GetStudentTranscript` and `fn_CalculateGPA`
  - `sp_GetDepartmentHierarchy` - Recursive CTE
  - Triggers (`trg_After_GradeUpdate`, `trg_InsteadOf_DeleteStudent`)

#### Factory Pattern
- `BusinessLogicFactory` - Creates appropriate service implementations
- Runtime switching between LINQ and SP implementations
- Enum `BLLImplementationType` { LINQ, StoredProcedure }

### 3. User Interface (UI)
- **StudentManagementSystem.UI** - WinForms application

#### Forms
- **MainForm** - Navigation hub with BLL implementation switcher
- **StudentForm** - Full CRUD for students with:
  - Add/Update/Delete operations
  - View transcript and GPA (uses functions)
  - Search functionality
  - Input validation
- **CourseForm** - Course management
- **EnrollmentForm** - Student enrollment with:
  - Register for courses (uses SP with hold/prereq checks)
  - Update grades (triggers audit log)
  - View available offerings

## Database Features Demonstrated

### ✅ Stored Procedures
- `sp_RegisterStudentForCourse` - Used in EnrollmentForm
- `sp_GetStudentTranscriptAndGPA` - Used in StudentForm
- `sp_GetDepartmentHierarchy` - Used in DepartmentService

### ✅ User-Defined Functions
- `fn_CheckPrerequisitesMet` - Called by registration SP
- `fn_CalculateGPA` - Used for GPA calculation
- `fn_GetStudentTranscript` - TVF for transcript view

### ✅ Triggers
- `trg_After_GradeUpdate` - Auto-populates `Audit_GradeChanges` table
- `trg_InsteadOf_DeleteStudent` - Soft delete (sets status to Inactive)

### ✅ Common Table Expressions (CTEs)
- Recursive CTE in `sp_GetDepartmentHierarchy`
- CTEs in functions (`fn_CheckPrerequisitesMet`, `fn_CalculateGPA`)

### ✅ Views
- `vw_StudentTranscript` - Used by both LINQ and SP implementations
- `vw_CourseCatalog` - Active courses by department
- `vw_AvailableCourseOfferings` - Open course sections

### ✅ Indexes
- Filtered: `IX_Students_Active`, `IX_Enrollments_FailedCourses`
- Unique: Course/Semester combinations, Student/Offering pairs
- FK helpers: StudentID, OfferingID, DepartmentID

### ✅ Table Partitioning
- `Enrollments` - Partitioned by `EnrollmentDate` (yearly)
- `Audit_GradeChanges` - Partitioned by `ChangeDate` (yearly)
- Partition functions: `PF_EnrollmentsByDate`, `PF_AuditByDate`

## Configuration

### Connection String
Edit `StudentManagementSystem.UI\App.config`:

```xml
<connectionStrings>
  <add name="StudentManagementDB" 
       connectionString="Server=localhost,1433;Database=StudentManagementDB;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### BLL Implementation Switcher
Set default implementation in `App.config`:

```xml
<appSettings>
  <!-- LINQ or StoredProcedure -->
  <add key="BLLImplementation" value="LINQ" />
</appSettings>
```

You can also switch at runtime using the "Switch BLL" button in MainForm.

## Setup Instructions

### 1. Database Setup
```sql
-- Run the Phase 1 SQL script first
-- This creates the database, tables, stored procedures, functions, triggers, etc.
sqlcmd -S localhost,1433 -U sa -P YourPassword123 -i StudentManagementDB_creation.sql
```

### 2. Docker SQL Server (if using placeholder connection)
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourPassword123" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest
```

### 3. Restore NuGet Packages
```powershell
cd StudentManagementSystem
nuget restore StudentManagementSystem.sln
```

### 4. Build Solution
- Open in Visual Studio 2022
- Build > Rebuild Solution
- Or use MSBuild:
```powershell
msbuild StudentManagementSystem.sln /t:Rebuild /p:Configuration=Debug
```

### 5. Run Application
```powershell
.\StudentManagementSystem.UI\bin\Debug\StudentManagementSystem.UI.exe
```

## Usage

### Switching BLL Implementations
1. Click "Switch BLL" button in MainForm
2. Current implementation shown in status label
3. All subsequent operations use the selected implementation

### Student Management
1. Click "Manage Students"
2. **Add**: Fill form, click "Add"
3. **Update**: Select student, modify fields, click "Update"
4. **Delete**: Select student, click "Delete" (soft delete via trigger)
5. **View Transcript**: Select student, click "View Transcript" (uses functions)
6. **Search**: Enter text, click "Search"

### Course Enrollment
1. Click "Manage Enrollments"
2. Select student and semester
3. **Register**: Click offering, click "Register" (uses SP with validations)
4. **Update Grade**: Select enrollment, enter grade, click "Update Grade" (triggers audit)
5. View student's current enrollments

### Input Validation
- Required fields: First Name, Last Name, Email, Status
- Email format validation
- Credit range validation (1-6)
- Date validations
- Unique constraint checks

## Testing BLL Implementations

### Test LINQ Implementation
1. Set App.config: `<add key="BLLImplementation" value="LINQ" />`
2. Add a student → Check with SQL: `SELECT * FROM Students WHERE Email = 'test@example.com'`
3. View transcript → Verify it queries `vw_StudentTranscript` view
4. Delete student → Verify status changed to 'Inactive' (trigger)

### Test Stored Procedure Implementation
1. Switch to SP implementation (button or config)
2. Register student for course → Verify SP called:
   ```sql
   -- Should fail if student has active holds
   -- Should check prerequisites
   -- Should verify capacity
   ```
3. Update grade → Check audit table:
   ```sql
   SELECT * FROM Audit_GradeChanges ORDER BY ChangeDate DESC
   ```

### Verify Partitioning
```sql
-- Check partition distribution
SELECT 
    $PARTITION.PF_EnrollmentsByDate(EnrollmentDate) AS PartitionNumber,
    COUNT(*) AS RowCount
FROM Enrollments
GROUP BY $PARTITION.PF_EnrollmentsByDate(EnrollmentDate)
```

## Key Features

1. **Factory Design Pattern** ✅
   - Runtime BLL switching
   - Loose coupling via interfaces

2. **Input Validation** ✅
   - Type checks
   - Required fields
   - Range validations
   - Email format

3. **Database Feature Integration** ✅
   - All 7 required features demonstrated
   - Meaningful interaction with each feature

4. **Error Handling** ✅
   - Try-catch blocks
   - User-friendly error messages
   - Transaction rollback on failures

5. **Clean Architecture** ✅
   - Separation of concerns
   - DAL → BLL → UI layers
   - No business logic in UI

## Project Structure
```
StudentManagementSystem.sln
├── StudentManagementSystem.DAL/
│   ├── Models/
│   │   ├── Student.cs
│   │   ├── Course.cs
│   │   ├── Enrollment.cs
│   │   └── ... (all entities)
│   ├── StudentManagementContext.cs
│   └── App.config
├── StudentManagementSystem.BLL/
│   ├── Interfaces/
│   │   ├── IStudentService.cs
│   │   └── ... (all service interfaces)
│   ├── LinqImplementation/
│   │   ├── LinqStudentService.cs
│   │   └── ... (LINQ services)
│   ├── StoredProcedureImplementation/
│   │   ├── SPStudentService.cs
│   │   └── ... (SP services)
│   ├── Factory/
│   │   └── BusinessLogicFactory.cs
│   └── App.config
└── StudentManagementSystem.UI/
    ├── Forms/
    │   ├── MainForm.cs
    │   ├── StudentForm.cs
    │   ├── CourseForm.cs
    │   └── EnrollmentForm.cs
    ├── Program.cs
    └── App.config
```

## Dependencies
- .NET Framework 4.8
- Entity Framework 6.4.4
- System.Data.SqlClient
- System.Windows.Forms
- System.Configuration

## Notes
- All forms implement proper disposal of database connections
- LINQ implementation uses `using` statements for DbContext
- SP implementation properly closes SqlConnection
- Triggers and functions are transparent to the application
- Partitioning is handled automatically by SQL Server

## Submission Checklist
- [x] Factory Design Pattern implemented
- [x] Two interchangeable BLL implementations (LINQ & SP)
- [x] Functional WinForms UI
- [x] Input validation on all forms
- [x] Runtime BLL switching capability
- [x] Uses all 7 required database features
- [x] Clean navigation between forms
- [x] Proper error handling
- [x] Connection string configuration
- [x] Documentation (this README)

## Author
Phase 2: Application Development
Deadline: December 7, 2025
