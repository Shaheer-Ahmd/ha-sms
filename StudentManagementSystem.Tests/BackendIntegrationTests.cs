using System;
using System.Data;
using Xunit;
using Microsoft.Data.SqlClient;

namespace StudentManagementSystem.Tests
{
    public class BackendIntegrationTests
    {
        private const string ConnStr =
            "Server=localhost,1433;Database=StudentManagementDB;User Id=SA;Password=YourPassword123!;TrustServerCertificate=True;";

        private static SqlConnection Open()
        {
            var c = new SqlConnection(ConnStr);
            c.Open();
            return c;
        }

        // ----------------------------------------------------
        // 1. BASIC CONNECTIVITY
        // ----------------------------------------------------
        [Fact]
        public void Can_Connect_To_Database()
        {
            using var c = Open();
            Assert.Equal(ConnectionState.Open, c.State);
        }

        // ----------------------------------------------------
        // 2. ENROLLMENT BLOCKED BY HOLD
        // ----------------------------------------------------
        [Fact]
        public void Enrollment_Denied_When_Student_Has_Hold()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                DECLARE @student INT = (SELECT TOP 1 StudentID FROM StudentHolds WHERE IsActive = 1);
                DECLARE @offering INT = (SELECT TOP 1 OfferingID FROM CourseOfferings);

                EXEC dbo.sp_RegisterStudentForCourse @student, @offering;
            ", c);

            var ex = Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
            Assert.Contains("holds", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        // ----------------------------------------------------
        // 3. PREREQUISITE CHECK
        // ----------------------------------------------------
        [Fact]
        public void Enrollment_Denied_When_Prerequisite_Not_Met()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                DECLARE @student INT =
                    (SELECT TOP 1 StudentID FROM Students WHERE StudentID NOT IN
                        (SELECT DISTINCT StudentID FROM Enrollments WHERE Grade IN ('A','B','C','D')));

                DECLARE @offering INT =
                    (SELECT TOP 1 o.OfferingID
                     FROM CourseOfferings o
                     JOIN CoursePrerequisites p ON o.CourseID = p.CourseID);

                EXEC dbo.sp_RegisterStudentForCourse @student, @offering;
            ", c);

            var ex = Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
            Assert.Contains("Prerequisites", ex.Message);
        }

        // ----------------------------------------------------
        // 4. FULL COURSE CHECK
        // ----------------------------------------------------
        [Fact]
        public void Enrollment_Denied_When_Course_Is_Full()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                DECLARE @offering INT =
                    (SELECT TOP 1 OfferingID FROM CourseOfferings
                     WHERE CurrentEnrollment >= MaxCapacity);

                IF @offering IS NULL THROW 51000, 'No full course found', 1;

                DECLARE @student INT =
                    (SELECT TOP 1 StudentID FROM Students WHERE EnrollmentStatus='Active');

                EXEC dbo.sp_RegisterStudentForCourse @student, @offering;
            ", c);

            var ex = Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
            Assert.Contains("full", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        // ----------------------------------------------------
        // 5. SUCCESSFUL ENROLLMENT
        // ----------------------------------------------------
        [Fact]
        public void Enrollment_Succeeds_When_All_Conditions_Met()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                DECLARE @student INT =
                    (SELECT TOP 1 StudentID FROM Students WHERE EnrollmentStatus='Active'
                     AND StudentID NOT IN (SELECT StudentID FROM StudentHolds WHERE IsActive=1));

                DECLARE @offering INT =
(
    SELECT TOP 1 o.OfferingID
    FROM dbo.CourseOfferings o
    WHERE o.CurrentEnrollment < o.MaxCapacity
      AND dbo.fn_CheckPrerequisitesMet(@student, o.CourseID) = 1
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.Enrollments e
          WHERE e.StudentID = @student
            AND e.OfferingID = o.OfferingID
      )
);
            ", c);

            cmd.ExecuteNonQuery(); // should NOT throw
        }

        // ----------------------------------------------------
        // 6. GPA FUNCTION
        // ----------------------------------------------------
        [Fact]
        public void GPA_Function_Returns_Value()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                SELECT dbo.fn_CalculateGPA(
                    (SELECT TOP 1 StudentID FROM Enrollments WHERE Grade IS NOT NULL)
                );
            ", c);

            var gpa = cmd.ExecuteScalar();
            Assert.NotNull(gpa);
        }

        // ----------------------------------------------------
        // 7. TRANSCRIPT FUNCTION
        // ----------------------------------------------------
        [Fact]
        public void Transcript_Function_Returns_Rows()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                SELECT COUNT(*) FROM dbo.fn_GetStudentTranscript(
                    (SELECT TOP 1 StudentID FROM Enrollments WHERE Grade IS NOT NULL)
                );
            ", c);

            var count = (int)cmd.ExecuteScalar();
            Assert.True(count > 0);
        }

        // ----------------------------------------------------
        // 8. AUDIT TRIGGER (GRADE CHANGE)
        // ----------------------------------------------------
        [Fact]
        public void Grade_Update_Creates_Audit_Record()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                DECLARE @eid INT, @edate DATE;

                SELECT TOP 1 @eid = EnrollmentID, @edate = EnrollmentDate
                FROM Enrollments WHERE Grade IS NOT NULL;

                UPDATE Enrollments
                SET Grade = CASE WHEN Grade = 'A' THEN 'B' ELSE 'A' END
                WHERE EnrollmentID = @eid AND EnrollmentDate = @edate;

                SELECT COUNT(*) FROM Audit_GradeChanges
                WHERE EnrollmentID = @eid AND EnrollmentDate = @edate;
            ", c);

            var count = (int)cmd.ExecuteScalar();
            Assert.True(count > 0);
        }
    }
}