/* ===========================================================
   Student Management System - Core Schema (Clean Version)
   - Drops & recreates StudentManagementDB from scratch
   - Creates tables, indexes, FKs, and extended properties
   =========================================================== */

PRINT '=== RUNNING StudentManagementDB_creation.sql v2 (no ChangedByAdminID) ===';
GO

---------------------------------------------------------------
-- 0. Drop & Recreate Database
---------------------------------------------------------------
IF DB_ID('StudentManagementDB') IS NOT NULL
BEGIN
    ALTER DATABASE StudentManagementDB 
        SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE StudentManagementDB;
END;
GO

CREATE DATABASE StudentManagementDB;
GO

USE StudentManagementDB;
GO

---------------------------------------------------------------
-- A. Partitioning Infrastructure
--    We�ll partition by date (year) for Enrollments & Audit_GradeChanges
---------------------------------------------------------------

-- 1) Partition Enrollments by EnrollmentDate
IF EXISTS (SELECT * FROM sys.partition_functions WHERE name = 'PF_EnrollmentsByDate')
    DROP PARTITION FUNCTION PF_EnrollmentsByDate;
IF EXISTS (SELECT * FROM sys.partition_schemes WHERE name = 'PS_EnrollmentsByDate')
    DROP PARTITION SCHEME PS_EnrollmentsByDate;
GO

-- Range RIGHT: each boundary starts a new partition
CREATE PARTITION FUNCTION PF_EnrollmentsByDate (DATE)
AS RANGE RIGHT FOR VALUES
(
    ('2022-01-01'),
    ('2023-01-01'),
    ('2024-01-01'),
    ('2025-01-01'),
    ('2026-01-01')
);
GO

CREATE PARTITION SCHEME PS_EnrollmentsByDate
AS PARTITION PF_EnrollmentsByDate
ALL TO ([PRIMARY]);
GO

-- 2) Partition Audit_GradeChanges by ChangeDate
IF EXISTS (SELECT * FROM sys.partition_functions WHERE name = 'PF_AuditByDate')
    DROP PARTITION FUNCTION PF_AuditByDate;
IF EXISTS (SELECT * FROM sys.partition_schemes WHERE name = 'PS_AuditByDate')
    DROP PARTITION SCHEME PS_AuditByDate;
GO

CREATE PARTITION FUNCTION PF_AuditByDate (DATE)
AS RANGE RIGHT FOR VALUES
(
    ('2022-01-01'),
    ('2023-01-01'),
    ('2024-01-01'),
    ('2025-01-01'),
    ('2026-01-01')
);
GO

CREATE PARTITION SCHEME PS_AuditByDate
AS PARTITION PF_AuditByDate
ALL TO ([PRIMARY]);
GO


---------------------------------------------------------------
-- 1. Core Tables
---------------------------------------------------------------

CREATE TABLE dbo.Departments (
    DepartmentID       INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName     NVARCHAR(255) NOT NULL UNIQUE,
    ParentDepartmentID INT NULL
);
GO

CREATE TABLE dbo.Students (
    StudentID        INT IDENTITY(1,1) PRIMARY KEY,
    FirstName        NVARCHAR(255) NOT NULL,
    LastName         NVARCHAR(255) NOT NULL,
    Email            NVARCHAR(255) NOT NULL UNIQUE,
    EnrollmentStatus NVARCHAR(50)  NOT NULL
        CONSTRAINT CK_Students_EnrollmentStatus
        CHECK (EnrollmentStatus IN ('Active', 'Inactive', 'Graduated', 'Suspended')),
    DateOfBirth      DATE NULL
);
GO

CREATE TABLE dbo.Semesters (
    SemesterID INT IDENTITY(1,1) PRIMARY KEY,
    [Year]     INT          NOT NULL,
    Season     NVARCHAR(50) NOT NULL
        CONSTRAINT CK_Semesters_Season
        CHECK (Season IN ('Fall', 'Spring', 'Summer'))
);
GO

CREATE TABLE dbo.Courses (
    CourseID      INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode    NVARCHAR(255) NOT NULL UNIQUE,
    Title         NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    Credits       INT NOT NULL
        CONSTRAINT CK_Courses_Credits CHECK (Credits BETWEEN 1 AND 6),
    DepartmentID  INT NOT NULL
);
GO

CREATE TABLE dbo.CoursePrerequisites (
    CourseID             INT NOT NULL,
    PrerequisiteCourseID INT NOT NULL,
    CONSTRAINT CK_CoursePrerequisites_NoSelf
        CHECK (CourseID <> PrerequisiteCourseID),
    CONSTRAINT PK_CoursePrerequisites
        PRIMARY KEY (CourseID, PrerequisiteCourseID)
);
GO

CREATE TABLE dbo.CourseOfferings (
    OfferingID        INT IDENTITY(1,1) PRIMARY KEY,
    CourseID          INT NOT NULL,
    SemesterID        INT NOT NULL,
    MaxCapacity       INT NOT NULL
        CONSTRAINT CK_CourseOfferings_MaxCapacity CHECK (MaxCapacity > 0),
    CurrentEnrollment INT NOT NULL
        CONSTRAINT DF_CourseOfferings_CurrentEnrollment DEFAULT (0),
    CONSTRAINT CK_CourseOfferings_CurrentEnrollment
        CHECK (CurrentEnrollment BETWEEN 0 AND MaxCapacity)
);
GO

CREATE TABLE dbo.Enrollments (
    EnrollmentID   INT IDENTITY(1,1) NOT NULL,
    StudentID      INT NOT NULL,
    OfferingID     INT NOT NULL,
    Grade          NVARCHAR(2) NULL  -- NULL for in-progress courses
        CONSTRAINT CK_Enrollments_Grade
        CHECK (Grade IN ('A', 'B', 'C', 'D', 'F') OR Grade IS NULL),
    EnrollmentDate DATE NOT NULL,
    CONSTRAINT PK_Enrollments
        PRIMARY KEY NONCLUSTERED (EnrollmentID, EnrollmentDate) ON [PRIMARY]
) ON PS_EnrollmentsByDate(EnrollmentDate);
GO


CREATE TABLE dbo.StudentHolds (
    HoldID      INT IDENTITY(1,1) PRIMARY KEY,
    StudentID   INT NOT NULL,
    HoldType    NVARCHAR(50) NOT NULL
        CONSTRAINT CK_StudentHolds_HoldType
        CHECK (HoldType IN ('Financial', 'Academic', 'Disciplinary', 'Administrative')),
    Reason      NVARCHAR(MAX) NULL,
    DateApplied DATETIME2(0) NOT NULL
        CONSTRAINT DF_StudentHolds_DateApplied DEFAULT (SYSUTCDATETIME())
);
GO

CREATE TABLE dbo.Audit_GradeChanges (
    AuditID        INT IDENTITY(1,1) NOT NULL,
    EnrollmentID   INT NOT NULL,
    EnrollmentDate DATE NOT NULL,
    OldGrade       NVARCHAR(2) NULL
        CONSTRAINT CK_Audit_GradeChanges_OldGrade
        CHECK (OldGrade IN ('A', 'B', 'C', 'D', 'F') OR OldGrade IS NULL),
    NewGrade       NVARCHAR(2) NULL
        CONSTRAINT CK_Audit_GradeChanges_NewGrade
        CHECK (NewGrade IN ('A', 'B', 'C', 'D', 'F') OR NewGrade IS NULL),
    ChangeDate     DATETIME2(0) NOT NULL
        CONSTRAINT DF_Audit_GradeChanges_ChangeDate DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_Audit_GradeChanges
        PRIMARY KEY NONCLUSTERED (AuditID) ON [PRIMARY]
) ON PS_AuditByDate(EnrollmentDate);
GO




---------------------------------------------------------------
-- 2. Indexes
---------------------------------------------------------------

-- Students: quick lookup of active students
CREATE INDEX IX_Students_Active
ON dbo.Students (EnrollmentStatus)
WHERE EnrollmentStatus = 'Active';
GO

-- Semesters: year + season unique
CREATE UNIQUE INDEX UQ_Semesters_Year_Season
ON dbo.Semesters ([Year], Season);
GO

-- Course offerings: each course appears at most once per semester
CREATE UNIQUE INDEX UQ_CourseOfferings_Course_Semester
ON dbo.CourseOfferings (CourseID, SemesterID);
GO

-- Enrollments: one enrollment per student per offering (non-partitioned index)
CREATE UNIQUE INDEX UQ_Enrollments_Student_Offering
ON dbo.Enrollments (StudentID, OfferingID)
ON [PRIMARY];
GO


-- Enrollments: fast failing-courses queries
CREATE INDEX IX_Enrollments_FailedCourses
ON dbo.Enrollments (StudentID, OfferingID)
WHERE Grade = 'F';
GO

---------------------------------------------------------------
-- 3. Extended Properties (Descriptions)
---------------------------------------------------------------

EXEC sys.sp_addextendedproperty
    @name = N'Table_Description',
    @value = N'Stores academic departments and their hierarchical structure (e.g., College contains CS, EE).',
    @level0type = N'Schema', @level0name = N'dbo',
    @level1type = N'Table',  @level1name = N'Departments';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Column_Description',
    @value = N'Self-reference for hierarchy',
    @level0type = N'Schema',  @level0name = N'dbo',
    @level1type = N'Table',   @level1name = N'Departments',
    @level2type = N'Column',  @level2name = N'ParentDepartmentID';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Table_Description',
    @value = N'Stores student personal information and enrollment status.',
    @level0type = N'Schema', @level0name = N'dbo',
    @level1type = N'Table',  @level1name = N'Students';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Table_Description',
    @value = N'Defines academic terms (e.g., Fall 2024, Spring 2025).',
    @level0type = N'Schema', @level0name = N'dbo',
    @level1type = N'Table',  @level1name = N'Semesters';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Table_Description',
    @value = N'Master catalog of all courses offered by the university.',
    @level0type = N'Schema', @level0name = N'dbo',
    @level1type = N'Table',  @level1name = N'Courses';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Column_Description',
    @value = N'e.g., "CS101"',
    @level0type = N'Schema',  @level0name = N'dbo',
    @level1type = N'Table',   @level1name = N'Courses',
    @level2type = N'Column',  @level2name = N'CourseCode';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Table_Description',
    @value = N'Stores the "rules" for prerequisite logic. (e.g., CS200 requires CS101).',
    @level0type = N'Schema', @level0name = N'dbo',
    @level1type = N'Table',  @level1name = N'CoursePrerequisites';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Table_Description',
    @value = N'Lists the specific courses available in a given semester with capacity tracking.',
    @level0type = N'Schema', @level0name = N'dbo',
    @level1type = N'Table',  @level1name = N'CourseOfferings';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Table_Description',
    @value = N'Connects a student to a specific course offering. Composite PK (EnrollmentID, EnrollmentDate) supports partitioning.',
    @level0type = N'Schema', @level0name = N'dbo',
    @level1type = N'Table',  @level1name = N'Enrollments';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Column_Description',
    @value = N'NULL is allowed for courses in progress',
    @level0type = N'Schema',  @level0name = N'dbo',
    @level1type = N'Table',   @level1name = N'Enrollments',
    @level2type = N'Column',  @level2name = N'Grade';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Column_Description',
    @value = N'Used as the partitioning key',
    @level0type = N'Schema',  @level0name = N'dbo',
    @level1type = N'Table',   @level1name = N'Enrollments',
    @level2type = N'Column',  @level2name = N'EnrollmentDate';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Table_Description',
    @value = N'Tracks administrative blocks preventing student registration (Financial, Academic, etc.).',
    @level0type = N'Schema', @level0name = N'dbo',
    @level1type = N'Table',  @level1name = N'StudentHolds';
GO

EXEC sys.sp_addextendedproperty
    @name = N'Table_Description',
    @value = N'Immutable audit trail for grade modifications. Populated by a grade-change trigger.',
    @level0type = N'Schema', @level0name = N'dbo',
    @level1type = N'Table',  @level1name = N'Audit_GradeChanges';
GO

---------------------------------------------------------------
-- 4. Foreign Keys
---------------------------------------------------------------

ALTER TABLE dbo.Departments
ADD CONSTRAINT FK_Departments_Parent
    FOREIGN KEY (ParentDepartmentID)
    REFERENCES dbo.Departments (DepartmentID);
GO

ALTER TABLE dbo.Courses
ADD CONSTRAINT FK_Courses_Departments
    FOREIGN KEY (DepartmentID)
    REFERENCES dbo.Departments (DepartmentID);
GO

ALTER TABLE dbo.CoursePrerequisites
ADD CONSTRAINT FK_CoursePrereqs_Course
    FOREIGN KEY (CourseID)
    REFERENCES dbo.Courses (CourseID);
GO

ALTER TABLE dbo.CoursePrerequisites
ADD CONSTRAINT FK_CoursePrereqs_Prereq
    FOREIGN KEY (PrerequisiteCourseID)
    REFERENCES dbo.Courses (CourseID);
GO

ALTER TABLE dbo.CourseOfferings
ADD CONSTRAINT FK_CourseOfferings_Course
    FOREIGN KEY (CourseID)
    REFERENCES dbo.Courses (CourseID);
GO

ALTER TABLE dbo.CourseOfferings
ADD CONSTRAINT FK_CourseOfferings_Semester
    FOREIGN KEY (SemesterID)
    REFERENCES dbo.Semesters (SemesterID);
GO

ALTER TABLE dbo.Enrollments
ADD CONSTRAINT FK_Enrollments_Student
    FOREIGN KEY (StudentID)
    REFERENCES dbo.Students (StudentID);
GO

ALTER TABLE dbo.Enrollments
ADD CONSTRAINT FK_Enrollments_Offering
    FOREIGN KEY (OfferingID)
    REFERENCES dbo.CourseOfferings (OfferingID);
GO

ALTER TABLE dbo.StudentHolds
ADD CONSTRAINT FK_StudentHolds_Student
    FOREIGN KEY (StudentID)
    REFERENCES dbo.Students (StudentID);
GO

ALTER TABLE dbo.Audit_GradeChanges
ADD CONSTRAINT FK_Audit_GradeChanges_Enrollment
    FOREIGN KEY (EnrollmentID, EnrollmentDate)
    REFERENCES dbo.Enrollments (EnrollmentID, EnrollmentDate)
    ON DELETE NO ACTION;
GO





/* ===========================================================
   Student Management System - Features Script
   (Run AFTER core tables are created)
   =========================================================== */

---------------------------------------------------------------
-- 0. Use target database & small schema tweaks
---------------------------------------------------------------
USE StudentManagementDB;
GO

-- Add IsActive flag to Departments (for Course Catalog view)
IF COL_LENGTH('dbo.Departments', 'IsActive') IS NULL
BEGIN
    ALTER TABLE dbo.Departments
    ADD IsActive BIT NOT NULL
        CONSTRAINT DF_Departments_IsActive DEFAULT (1);
END;
GO

-- Add IsActive flag to StudentHolds (for active holds logic + index)
IF COL_LENGTH('dbo.StudentHolds', 'IsActive') IS NULL
BEGIN
    ALTER TABLE dbo.StudentHolds
    ADD IsActive BIT NOT NULL
        CONSTRAINT DF_StudentHolds_IsActive DEFAULT (1);
END;
GO

/* ===========================================================
   1. VIEWS
   =========================================================== */

-------------------------------
-- 1.1 vw_CourseCatalog
-------------------------------
IF OBJECT_ID('dbo.vw_CourseCatalog', 'V') IS NOT NULL
    DROP VIEW dbo.vw_CourseCatalog;
GO

CREATE VIEW dbo.vw_CourseCatalog
AS
SELECT
    c.CourseID,
    c.CourseCode,
    c.Title       AS CourseTitle,
    c.Credits,
    d.DepartmentID,
    d.DepartmentName,
    d.IsActive
FROM dbo.Courses     AS c
JOIN dbo.Departments AS d
    ON c.DepartmentID = d.DepartmentID
WHERE d.IsActive = 1;
GO

-------------------------------
-- 1.2 vw_AvailableCourseOfferings
-------------------------------
IF OBJECT_ID('dbo.vw_AvailableCourseOfferings', 'V') IS NOT NULL
    DROP VIEW dbo.vw_AvailableCourseOfferings;
GO

CREATE VIEW dbo.vw_AvailableCourseOfferings
AS
SELECT
    o.OfferingID,
    c.CourseCode,
    c.Title        AS CourseTitle,
    s.[Year],
    s.Season,
    o.MaxCapacity,
    o.CurrentEnrollment,
    (o.MaxCapacity - o.CurrentEnrollment) AS SeatsRemaining
FROM dbo.CourseOfferings AS o
JOIN dbo.Courses         AS c ON o.CourseID   = c.CourseID
JOIN dbo.Semesters       AS s ON o.SemesterID = s.SemesterID
WHERE o.CurrentEnrollment < o.MaxCapacity;
GO

-------------------------------
-- 1.3 vw_StudentTranscript
-------------------------------
IF OBJECT_ID('dbo.vw_StudentTranscript', 'V') IS NOT NULL
    DROP VIEW dbo.vw_StudentTranscript;
GO

CREATE VIEW dbo.vw_StudentTranscript
AS
SELECT
    st.StudentID,
    st.FirstName,
    st.LastName,
    c.CourseCode,
    c.Title     AS CourseTitle,
    c.Credits,
    s.[Year],
    s.Season,
    e.Grade
FROM dbo.Enrollments      AS e
JOIN dbo.Students         AS st ON e.StudentID   = st.StudentID
JOIN dbo.CourseOfferings  AS o  ON e.OfferingID  = o.OfferingID
JOIN dbo.Courses          AS c  ON o.CourseID    = c.CourseID
JOIN dbo.Semesters        AS s  ON o.SemesterID  = s.SemesterID
WHERE e.Grade IS NOT NULL;   -- completed courses only
GO

/* ===========================================================
   2. FUNCTIONS
   =========================================================== */

-------------------------------
-- 2.1 fn_CheckPrerequisitesMet (@StudentID, @CourseID)
-------------------------------
IF OBJECT_ID('dbo.fn_CheckPrerequisitesMet', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_CheckPrerequisitesMet;
GO

CREATE FUNCTION dbo.fn_CheckPrerequisitesMet
(
    @StudentID INT,
    @CourseID  INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @result BIT = 1;

    -- Only check if the course actually has prerequisites
    IF EXISTS (
        SELECT 1
        FROM dbo.CoursePrerequisites
        WHERE CourseID = @CourseID
    )
    BEGIN
        ;WITH Needed AS (
            -- All required prerequisite courses
            SELECT cp.PrerequisiteCourseID AS PrereqCourseID
            FROM dbo.CoursePrerequisites AS cp
            WHERE cp.CourseID = @CourseID
        ),
        Taken AS (
            -- All courses this student has passed (A�D)
            SELECT DISTINCT c.CourseID
            FROM dbo.Enrollments     AS e
            JOIN dbo.CourseOfferings AS o ON e.OfferingID = o.OfferingID
            JOIN dbo.Courses         AS c ON o.CourseID   = c.CourseID
            WHERE e.StudentID = @StudentID
              AND e.Grade IN ('A','B','C','D')
        )
        -- Single statement after CTE: set @result
        SELECT @result =
            CASE 
                WHEN EXISTS (
                    SELECT 1
                    FROM Needed AS n
                    LEFT JOIN Taken AS t
                        ON n.PrereqCourseID = t.CourseID
                    WHERE t.CourseID IS NULL   -- missing at least one prereq
                )
                THEN 0
                ELSE 1
            END;
    END

    RETURN @result;
END;
GO

-------------------------------
-- 2.2 fn_CalculateGPA (@StudentID)
-------------------------------
IF OBJECT_ID('dbo.fn_CalculateGPA', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_CalculateGPA;
GO

CREATE FUNCTION dbo.fn_CalculateGPA
(
    @StudentID INT
)
RETURNS DECIMAL(3,2)
AS
BEGIN
    DECLARE @gpa DECIMAL(5,4);

    ;WITH StudentGrades AS (
        SELECT
            c.Credits,
            e.Grade,
            CASE e.Grade
                WHEN 'A' THEN 4.0
                WHEN 'B' THEN 3.0
                WHEN 'C' THEN 2.0
                WHEN 'D' THEN 1.0
                WHEN 'F' THEN 0.0
            END AS GradePoints
        FROM dbo.Enrollments     AS e
        JOIN dbo.CourseOfferings AS o ON e.OfferingID = o.OfferingID
        JOIN dbo.Courses         AS c ON o.CourseID   = c.CourseID
        WHERE e.StudentID = @StudentID
          AND e.Grade IS NOT NULL         -- ignore in-progress courses
    )
    SELECT @gpa =
        CASE WHEN SUM(Credits) = 0 THEN NULL
             ELSE SUM(GradePoints * Credits) / NULLIF(SUM(Credits),0)
        END
    FROM StudentGrades;

    RETURN CONVERT(DECIMAL(3,2), @gpa);
END;
GO

-------------------------------
-- 2.3 fn_GetStudentTranscript (@StudentID) - inline TVF
-------------------------------
IF OBJECT_ID('dbo.fn_GetStudentTranscript', 'IF') IS NOT NULL
    DROP FUNCTION dbo.fn_GetStudentTranscript;
GO

CREATE FUNCTION dbo.fn_GetStudentTranscript
(
    @StudentID INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        vt.CourseCode,
        vt.CourseTitle,
        vt.Credits,
        vt.[Year],
        vt.Season,
        vt.Grade
    FROM dbo.vw_StudentTranscript AS vt
    WHERE vt.StudentID = @StudentID
);
GO

/* ===========================================================
   3. STORED PROCEDURES
   =========================================================== */

-------------------------------
-- 3.1 sp_RegisterStudentForCourse
-------------------------------
IF OBJECT_ID('dbo.sp_RegisterStudentForCourse', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_RegisterStudentForCourse;
GO

CREATE PROCEDURE dbo.sp_RegisterStudentForCourse
    @StudentID  INT,
    @OfferingID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE
        @courseID       INT,
        @maxCap         INT,
        @currentEnroll  INT,
        @prereqsOk      BIT;

    -- 1. Check active holds
    IF EXISTS (
        SELECT 1
        FROM dbo.StudentHolds
        WHERE StudentID = @StudentID
          AND IsActive  = 1
    )
    BEGIN
        RAISERROR('Student has active holds. Registration denied.', 16, 1);
        RETURN;
    END

    -- 2. Check offering and capacity
    SELECT
        @courseID      = o.CourseID,
        @maxCap        = o.MaxCapacity,
        @currentEnroll = o.CurrentEnrollment
    FROM dbo.CourseOfferings AS o
    WHERE o.OfferingID = @OfferingID;

    IF @courseID IS NULL
    BEGIN
        RAISERROR('Invalid OfferingID.', 16, 1);
        RETURN;
    END

    IF @currentEnroll >= @maxCap
    BEGIN
        RAISERROR('Course is full.', 16, 1);
        RETURN;
    END

    -- 3. Check prerequisites
    SET @prereqsOk = dbo.fn_CheckPrerequisitesMet(@StudentID, @courseID);

    IF @prereqsOk = 0
    BEGIN
        RAISERROR('Prerequisites not satisfied.', 16, 1);
        RETURN;
    END

    -- 4. Atomic registration
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO dbo.Enrollments (StudentID, OfferingID, Grade, EnrollmentDate)
        VALUES (@StudentID, @OfferingID, NULL, CONVERT(date, GETDATE()));

        UPDATE dbo.CourseOfferings
        SET CurrentEnrollment = CurrentEnrollment + 1
        WHERE OfferingID = @OfferingID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @msg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Registration failed: %s', 16, 1, @msg);
    END CATCH
END;
GO

-------------------------------
-- 3.2 sp_GetDepartmentHierarchy
-------------------------------
IF OBJECT_ID('dbo.sp_GetDepartmentHierarchy', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetDepartmentHierarchy;
GO

CREATE PROCEDURE dbo.sp_GetDepartmentHierarchy
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH DepartmentHierarchy AS (
        -- Root (no parent)
        SELECT
            d.DepartmentID,
            d.DepartmentName,
            d.ParentDepartmentID,
            0 AS Level,
            CAST(d.DepartmentName AS NVARCHAR(4000)) AS HierarchyPath
        FROM dbo.Departments AS d
        WHERE d.ParentDepartmentID IS NULL

        UNION ALL

        -- Children
        SELECT
            child.DepartmentID,
            child.DepartmentName,
            child.ParentDepartmentID,
            parent.Level + 1,
            CAST(parent.HierarchyPath + N' > ' + child.DepartmentName AS NVARCHAR(4000))
        FROM dbo.Departments AS child
        JOIN DepartmentHierarchy AS parent
            ON child.ParentDepartmentID = parent.DepartmentID
    )
    SELECT
        DepartmentID,
        REPLICATE(' ', Level * 4) + DepartmentName AS IndentedName,
        Level,
        HierarchyPath
    FROM DepartmentHierarchy
    ORDER BY HierarchyPath
    OPTION (MAXRECURSION 100);
END;
GO

-------------------------------
-- 3.3 sp_GetStudentTranscriptAndGPA
-------------------------------
IF OBJECT_ID('dbo.sp_GetStudentTranscriptAndGPA', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetStudentTranscriptAndGPA;
GO

CREATE PROCEDURE dbo.sp_GetStudentTranscriptAndGPA
    @StudentID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Transcript
    SELECT *
    FROM dbo.fn_GetStudentTranscript(@StudentID);

    -- GPA
    SELECT dbo.fn_CalculateGPA(@StudentID) AS GPA;
END;
GO

/* ===========================================================
   4. TRIGGERS
   =========================================================== */

-------------------------------
-- 4.1 trg_After_GradeUpdate on Enrollments
-------------------------------
IF OBJECT_ID('dbo.trg_After_GradeUpdate', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_After_GradeUpdate;
GO

CREATE TRIGGER dbo.trg_After_GradeUpdate
ON dbo.Enrollments
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT UPDATE(Grade)
        RETURN;

    INSERT INTO dbo.Audit_GradeChanges
        (EnrollmentID, EnrollmentDate, OldGrade, NewGrade)
    SELECT
        i.EnrollmentID,
        i.EnrollmentDate,
        d.Grade AS OldGrade,
        i.Grade AS NewGrade
    FROM inserted AS i
    JOIN deleted  AS d
        ON i.EnrollmentID   = d.EnrollmentID
       AND i.EnrollmentDate = d.EnrollmentDate
    WHERE ISNULL(d.Grade, '') <> ISNULL(i.Grade, '');  -- ignore no-op updates
END;
GO


-------------------------------
-- 4.2 trg_InsteadOf_DeleteStudent on Students
-------------------------------
IF OBJECT_ID('dbo.trg_InsteadOf_DeleteStudent', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_InsteadOf_DeleteStudent;
GO

CREATE TRIGGER dbo.trg_InsteadOf_DeleteStudent
ON dbo.Students
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Soft delete: mark as Inactive instead of physical delete
    UPDATE s
    SET EnrollmentStatus = 'Inactive'
    FROM dbo.Students AS s
    JOIN deleted      AS d
        ON s.StudentID = d.StudentID;

    -- Optionally, more logic (e.g., cancel future enrollments) can go here
END;
GO

/* ===========================================================
   5. SUPPORTING INDEXES (FK + FILTERED)
   =========================================================== */

-- Foreign-key helper indexes
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Enrollments_StudentID'
      AND object_id = OBJECT_ID('dbo.Enrollments')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Enrollments_StudentID
    ON dbo.Enrollments (StudentID);
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Enrollments_OfferingID'
      AND object_id = OBJECT_ID('dbo.Enrollments')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Enrollments_OfferingID
    ON dbo.Enrollments (OfferingID);
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Courses_DepartmentID'
      AND object_id = OBJECT_ID('dbo.Courses')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Courses_DepartmentID
    ON dbo.Courses (DepartmentID);
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_StudentHolds_Student_IsActive'
      AND object_id = OBJECT_ID('dbo.StudentHolds')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_StudentHolds_Student_IsActive
    ON dbo.StudentHolds (StudentID, IsActive);
END;
GO

-- Filtered failing-courses index
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Enrollments_Failing'
      AND object_id = OBJECT_ID('dbo.Enrollments')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Enrollments_Failing
    ON dbo.Enrollments (StudentID, OfferingID)
    WHERE Grade = 'F';
END;
GO

-- Filtered active students index (if not already present)
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Students_Active'
      AND object_id = OBJECT_ID('dbo.Students')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Students_Active
    ON dbo.Students (EnrollmentStatus)
    WHERE EnrollmentStatus = 'Active';
END;
GO



USE StudentManagementDB;
GO
SET NOCOUNT ON;

PRINT '--- STARTING DATA POPULATION ---';

---------------------------------------------------------------
-- 1. Departments (small, hierarchical)
---------------------------------------------------------------
PRINT 'Step 1: Inserting Departments...';

IF NOT EXISTS (SELECT 1 FROM dbo.Departments)
BEGIN
    INSERT INTO dbo.Departments (DepartmentName, ParentDepartmentID)
    VALUES
        ('College of Engineering', NULL),  -- parent
        ('Computer Science', 1),
        ('Electrical Engineering', 1),
        ('Mechanical Engineering', 1),
        ('Civil Engineering', 1);
END;
GO

---------------------------------------------------------------
-- 2. Semesters (small reference set)
---------------------------------------------------------------
PRINT 'Step 2: Inserting Semesters...';

IF NOT EXISTS (SELECT 1 FROM dbo.Semesters)
BEGIN
    -- Years 2021�2025, 3 seasons each
    INSERT INTO dbo.Semesters ([Year], Season)
    VALUES
        (2021, 'Spring'), (2021, 'Summer'), (2021, 'Fall'),
        (2022, 'Spring'), (2022, 'Summer'), (2022, 'Fall'),
        (2023, 'Spring'), (2023, 'Summer'), (2023, 'Fall'),
        (2024, 'Spring'), (2024, 'Summer'), (2024, 'Fall'),
        (2025, 'Spring'), (2025, 'Summer'), (2025, 'Fall');
END;
GO

---------------------------------------------------------------
-- 3. Courses (~500)
---------------------------------------------------------------
PRINT 'Step 3: Inserting Courses...';

IF NOT EXISTS (SELECT 1 FROM dbo.Courses)
BEGIN
    DECLARE @DeptCount INT  = (SELECT COUNT(*) FROM dbo.Departments);
    DECLARE @MinDeptID INT  = (SELECT MIN(DepartmentID) FROM dbo.Departments);

    ;WITH Numbers AS (
        SELECT TOP (500)
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
        FROM sys.all_objects a
        CROSS JOIN sys.all_objects b
    )
    INSERT INTO dbo.Courses (CourseCode, Title, [Description], Credits, DepartmentID)
    SELECT
        CONCAT('C', FORMAT(n, '0000')) AS CourseCode,
        CONCAT('Course ', n) AS Title,
        CONCAT('Description for course ', n) AS [Description],
        (n % 6) + 1 AS Credits,                  -- 1�6
        @MinDeptID + ((n - 1) % @DeptCount)     -- spread across departments
    FROM Numbers;
END;
GO

---------------------------------------------------------------
-- 4. Course Prerequisites (chain for first ~300 courses)
---------------------------------------------------------------
PRINT 'Step 4: Inserting CoursePrerequisites...';

IF NOT EXISTS (SELECT 1 FROM dbo.CoursePrerequisites)
BEGIN
    INSERT INTO dbo.CoursePrerequisites (CourseID, PrerequisiteCourseID)
    SELECT c.CourseID, c.CourseID - 1
    FROM dbo.Courses AS c
    WHERE c.CourseID > (SELECT MIN(CourseID) FROM dbo.Courses)
      AND c.CourseID <= (SELECT MIN(CourseID) + 300 FROM dbo.Courses);
END;
GO

---------------------------------------------------------------
-- 5. Course Offerings (~Courses x Semesters)
---------------------------------------------------------------
PRINT 'Step 5: Inserting CourseOfferings...';

IF NOT EXISTS (SELECT 1 FROM dbo.CourseOfferings)
BEGIN
    INSERT INTO dbo.CourseOfferings (CourseID, SemesterID, MaxCapacity, CurrentEnrollment)
    SELECT
        c.CourseID,
        s.SemesterID,
        50 + (ABS(CHECKSUM(c.CourseID, s.SemesterID)) % 151) AS MaxCapacity,  -- 50�200
        0 AS CurrentEnrollment
    FROM dbo.Courses   AS c
    CROSS JOIN dbo.Semesters AS s;
END;
GO

---------------------------------------------------------------
-- 6. Students (~100,000)
---------------------------------------------------------------
PRINT 'Step 6: Inserting Students (~100k)...';

IF NOT EXISTS (SELECT 1 FROM dbo.Students)
BEGIN
    ;WITH Numbers AS (
        SELECT TOP (100000)
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
        FROM sys.all_objects a
        CROSS JOIN sys.all_objects b
    )
    INSERT INTO dbo.Students (FirstName, LastName, Email, EnrollmentStatus, DateOfBirth)
    SELECT
        CONCAT('First', n) AS FirstName,
        CONCAT('Last',  n) AS LastName,
        CONCAT('student', n, '@univ.edu') AS Email,
        CASE (n % 4)
            WHEN 0 THEN 'Active'
            WHEN 1 THEN 'Inactive'
            WHEN 2 THEN 'Graduated'
            ELSE 'Suspended'
        END AS EnrollmentStatus,
        DATEADD(DAY, -(n % 10000), CAST('2005-01-01' AS DATE)) AS DateOfBirth
    FROM Numbers;
END;
GO

---------------------------------------------------------------
-- 7. Enrollments (~900,000)
---------------------------------------------------------------
PRINT 'Step 7: Inserting Enrollments (~900k)...';

IF NOT EXISTS (SELECT 1 FROM dbo.Enrollments)
BEGIN
    DECLARE @StudentCount    INT  = (SELECT COUNT(*) FROM dbo.Students);
    DECLARE @OfferingCount   INT  = (SELECT COUNT(*) FROM dbo.CourseOfferings);
    DECLARE @MinStudentID    INT  = (SELECT MIN(StudentID) FROM dbo.Students);
    DECLARE @MinOfferingID   INT  = (SELECT MIN(OfferingID) FROM dbo.CourseOfferings);

    -- Target total enrollments (make sure it's less than StudentCount * OfferingCount)
    DECLARE @TotalEnrollments INT = 900000;

    IF @StudentCount * @OfferingCount < @TotalEnrollments
    BEGIN
        -- Safety fallback if there aren't enough combinations
        SET @TotalEnrollments = @StudentCount * @OfferingCount;
    END;

    ;WITH Numbers AS (
        SELECT TOP (@TotalEnrollments)
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 AS n
        FROM sys.all_objects a
        CROSS JOIN sys.all_objects b
    )
    INSERT INTO dbo.Enrollments (StudentID, OfferingID, Grade, EnrollmentDate)
    SELECT
        @MinStudentID + (n % @StudentCount)    AS StudentID,
        @MinOfferingID + (n / @StudentCount)   AS OfferingID,
        CASE (ABS(CHECKSUM(n)) % 10)
            WHEN 0 THEN 'A'
            WHEN 1 THEN 'A'
            WHEN 2 THEN 'B'
            WHEN 3 THEN 'B'
            WHEN 4 THEN 'C'
            WHEN 5 THEN 'C'
            WHEN 6 THEN 'D'
            WHEN 7 THEN 'F'
            ELSE NULL      -- some courses "in progress"
        END AS Grade,
        DATEADD(DAY, -(n % 1460), CAST(GETDATE() AS DATE)) AS EnrollmentDate  -- within last ~4 years
    FROM Numbers;
END;
GO

---------------------------------------------------------------
-- 8. Student Holds (~5,000)
---------------------------------------------------------------
PRINT 'Step 8: Inserting StudentHolds (~5k)...';

IF NOT EXISTS (SELECT 1 FROM dbo.StudentHolds)
BEGIN
    INSERT INTO dbo.StudentHolds (StudentID, HoldType, Reason)
    SELECT TOP (5000)
        s.StudentID,
        CASE (s.StudentID % 4)
            WHEN 0 THEN 'Financial'
            WHEN 1 THEN 'Academic'
            WHEN 2 THEN 'Disciplinary'
            ELSE 'Administrative'
        END AS HoldType,
        CASE (s.StudentID % 4)
            WHEN 0 THEN 'Outstanding tuition balance'
            WHEN 1 THEN 'GPA below threshold'
            WHEN 2 THEN 'Code of conduct violation'
            ELSE 'Administrative review pending'
        END AS Reason
    FROM dbo.Students AS s
    WHERE s.StudentID % 20 = 0        -- sparse subset of students
    ORDER BY s.StudentID;
END;
GO

---------------------------------------------------------------
-- 9. Audit_GradeChanges (~20,000)
---------------------------------------------------------------
PRINT 'Step 9: Inserting Audit_GradeChanges (~20k)...';

IF NOT EXISTS (SELECT 1 FROM dbo.Audit_GradeChanges)
BEGIN
    -- Take a subset of enrollments that have a non-NULL grade
    INSERT INTO dbo.Audit_GradeChanges
        (EnrollmentID, EnrollmentDate, OldGrade, NewGrade)
    SELECT TOP (20000)
        e.EnrollmentID,
        e.EnrollmentDate,
        CASE e.Grade
            WHEN 'A' THEN 'B'
            WHEN 'B' THEN 'C'
            WHEN 'C' THEN 'D'
            WHEN 'D' THEN 'F'
            WHEN 'F' THEN 'F'
            ELSE NULL
        END AS OldGrade,
        e.Grade AS NewGrade
    FROM dbo.Enrollments AS e
    WHERE e.Grade IS NOT NULL
    ORDER BY e.EnrollmentID;
END;
GO


---------------------------------------------------------------
-- 10. Summary (fixed)
---------------------------------------------------------------
PRINT 'Step 10: Summary row counts...';

SELECT 'Departments'        AS TableName, COUNT(*) FROM dbo.Departments
UNION ALL
SELECT 'Semesters'          AS TableName, COUNT(*) FROM dbo.Semesters
UNION ALL
SELECT 'Courses'            AS TableName, COUNT(*) FROM dbo.Courses
UNION ALL
SELECT 'CoursePrereqs'      AS TableName, COUNT(*) FROM dbo.CoursePrerequisites
UNION ALL
SELECT 'CourseOfferings'    AS TableName, COUNT(*) FROM dbo.CourseOfferings
UNION ALL
SELECT 'Students'           AS TableName, COUNT(*) FROM dbo.Students
UNION ALL
SELECT 'Enrollments'        AS TableName, COUNT(*) FROM dbo.Enrollments
UNION ALL
SELECT 'StudentHolds'       AS TableName, COUNT(*) FROM dbo.StudentHolds
UNION ALL
SELECT 'Audit_GradeChanges' AS TableName, COUNT(*) FROM dbo.Audit_GradeChanges;
GO

PRINT '--- DATA POPULATION COMPLETE ---';

