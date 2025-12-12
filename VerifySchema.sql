/* ===========================================================
   VerifySchema.sql
   Checks if specific Partitions and Indexes exist in StudentManagementDB
   =========================================================== */
USE StudentManagementDB;
GO
SET NOCOUNT ON;

PRINT '=============================================================';
PRINT '   DATABASE INTEGRITY CHECK REPORT';
PRINT '=============================================================';
PRINT '';

---------------------------------------------------
-- 1. CHECK PARTITION SCHEMES & FUNCTIONS
---------------------------------------------------
PRINT '--- [1] Checking Partition Infrastructure ---';

DECLARE @PF_Enrollment INT = (SELECT 1 FROM sys.partition_functions WHERE name = 'PF_EnrollmentsByDate');
DECLARE @PS_Enrollment INT = (SELECT 1 FROM sys.partition_schemes WHERE name = 'PS_EnrollmentsByDate');
DECLARE @PF_Audit INT = (SELECT 1 FROM sys.partition_functions WHERE name = 'PF_AuditByDate');
DECLARE @PS_Audit INT = (SELECT 1 FROM sys.partition_schemes WHERE name = 'PS_AuditByDate');

PRINT 'PF_EnrollmentsByDate: ' + CASE WHEN @PF_Enrollment = 1 THEN 'OK' ELSE 'MISSING' END;
PRINT 'PS_EnrollmentsByDate: ' + CASE WHEN @PS_Enrollment = 1 THEN 'OK' ELSE 'MISSING' END;
PRINT 'PF_AuditByDate:       ' + CASE WHEN @PF_Audit = 1 THEN 'OK' ELSE 'MISSING' END;
PRINT 'PS_AuditByDate:       ' + CASE WHEN @PS_Audit = 1 THEN 'OK' ELSE 'MISSING' END;
PRINT '';

---------------------------------------------------
-- 2. VERIFY DATA DISTRIBUTION (Did partitioning actually work?)
---------------------------------------------------
PRINT '--- [2] Verifying Data Distribution (Row Counts per Partition) ---';

SELECT 
    t.name AS TableName,
    i.name AS IndexName,
    p.partition_number,
    p.rows AS RowCount_In_Partition
FROM sys.partitions p
JOIN sys.tables t ON p.object_id = t.object_id
JOIN sys.indexes i ON p.object_id = i.object_id AND p.index_id = i.index_id
WHERE t.name IN ('Enrollments', 'Audit_GradeChanges')
AND i.type_desc IN ('CLUSTERED', 'HEAP') -- Focus on the main storage
ORDER BY t.name, p.partition_number;

PRINT '';

---------------------------------------------------
-- 3. CHECK SPECIFIC INDEXES
---------------------------------------------------
PRINT '--- [3] Checking Critical & Filtered Indexes ---';

DECLARE @ExpectedIndexes TABLE (TableName NVARCHAR(50), IndexName NVARCHAR(100));
INSERT INTO @ExpectedIndexes VALUES 
('Students', 'IX_Students_Active'),
('Semesters', 'UQ_Semesters_Year_Season'),
('CourseOfferings', 'UQ_CourseOfferings_Course_Semester'),
('Enrollments', 'UQ_Enrollments_Student_Offering'),
('Enrollments', 'IX_Enrollments_FailedCourses'),
('Enrollments', 'IX_Enrollments_Failing'), -- You had two failing indexes in your script
('Enrollments', 'IX_Enrollments_StudentID'),
('Enrollments', 'IX_Enrollments_OfferingID'),
('Courses', 'IX_Courses_DepartmentID'),
('StudentHolds', 'IX_StudentHolds_Student_IsActive');

SELECT 
    E.TableName, 
    E.IndexName, 
    Status = CASE WHEN I.name IS NOT NULL THEN 'CREATED' ELSE 'MISSING !!!' END
FROM @ExpectedIndexes E
LEFT JOIN sys.indexes I 
    ON I.name = E.IndexName 
    AND I.object_id = OBJECT_ID(E.TableName);

PRINT '';
PRINT '=============================================================';
PRINT '   END REPORT';
PRINT '=============================================================';
GO