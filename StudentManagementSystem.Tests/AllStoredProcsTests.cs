using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Xunit;

namespace StudentManagementSystem.Tests
{
    public class AllStoredProceduresIntegrationTests
    {
        private const string ConnectionString =
            "Server=localhost,1433;Database=StudentManagementDB;User Id=SA;Password=YourPassword123!;TrustServerCertificate=True;";

        private SqlConnection GetConnection()
            => new SqlConnection(ConnectionString);

        /* =========================================================
           HELPER
           ========================================================= */

        private int ExecuteCount(string storedProcedure, params SqlParameter[] parameters)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(storedProcedure, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read()) count++;
            return count;
        }

        /* =========================================================
           STUDENT STORED PROCEDURES
           ========================================================= */

        [Fact]
        public void Student_SP_GetAll_Works()
        {
            var count = ExecuteCount("sp_Student_GetAll");
            Assert.True(count > 0);
        }

        [Fact]
        public void Student_SP_GetActive_Works()
        {
            var count = ExecuteCount("sp_Student_GetActive");
            Assert.True(count > 0);
        }

        [Fact]
        public void Student_SP_Search_Works()
        {
            var count = ExecuteCount(
                "sp_Student_Search",
                new SqlParameter("@SearchTerm", "First")
            );
            Assert.True(count > 0);
        }

        /* =========================================================
           DEPARTMENT STORED PROCEDURES
           ========================================================= */

        [Fact]
        public void Department_SP_GetAll_Works()
        {
            var count = ExecuteCount("sp_Department_GetAll");
            Assert.True(count > 0);
        }

        [Fact]
        public void Department_SP_GetActive_Works()
        {
            var count = ExecuteCount("sp_Department_GetActive");
            Assert.True(count > 0);
        }

        /* =========================================================
           COURSE STORED PROCEDURES
           ========================================================= */

        [Fact]
        public void Course_SP_GetAll_Works()
        {
            var count = ExecuteCount("sp_Course_GetAll");
            Assert.True(count > 0);
        }

        [Fact]
        public void Course_SP_GetCatalog_Works()
        {
            var count = ExecuteCount("sp_Course_GetCatalog");
            Assert.True(count > 0);
        }

        /* =========================================================
           COURSE OFFERING STORED PROCEDURES
           ========================================================= */

        [Fact]
        public void CourseOffering_SP_GetAll_Works()
        {
            var count = ExecuteCount("sp_CourseOffering_GetAll");
            Assert.True(count > 0);
        }

        [Fact]
        public void CourseOffering_SP_GetAvailable_Works()
        {
            var count = ExecuteCount("sp_CourseOffering_GetAvailable");
            Assert.True(count > 0);
        }

        /* =========================================================
           ENROLLMENT STORED PROCEDURES
           ========================================================= */

        [Fact]
        public void Enrollment_SP_GetAll_Works()
        {
            var count = ExecuteCount("sp_Enrollment_GetAll");
            Assert.True(count > 0);
        }

        [Fact]
        public void Enrollment_SP_GetFailing_Works()
        {
            var count = ExecuteCount("sp_Enrollment_GetFailing");
            Assert.True(count >= 0); // may be zero, just must not fail
        }

        /* =========================================================
           SEMESTER STORED PROCEDURES
           ========================================================= */

        [Fact]
        public void Semester_SP_GetAll_Works()
        {
            var count = ExecuteCount("sp_Semester_GetAll");
            Assert.True(count > 0);
        }

        [Fact]
        public void Semester_SP_GetCurrent_Works()
        {
            var count = ExecuteCount("sp_Semester_GetCurrent");
            Assert.Equal(1, count);
        }

        /* =========================================================
           STUDENT HOLD STORED PROCEDURES
           ========================================================= */

        [Fact]
        public void StudentHold_SP_GetAll_Works()
        {
            var count = ExecuteCount("sp_StudentHold_GetAll");
            Assert.True(count > 0);
        }

        /* =========================================================
           PREREQUISITE STORED PROCEDURES
           ========================================================= */

        [Fact]
        public void CoursePrerequisite_SP_GetByCourse_Works()
        {
            using var conn = GetConnection();
            conn.Open();

            var courseIdCmd = new SqlCommand(
                "SELECT TOP 1 CourseID FROM CoursePrerequisites", conn);
            var courseId = (int)courseIdCmd.ExecuteScalar();

            var count = ExecuteCount(
                "sp_CoursePrerequisite_GetByCourse",
                new SqlParameter("@CourseID", courseId)
            );

            Assert.True(count > 0);
        }

        /* =========================================================
           AUDIT STORED PROCEDURES
           ========================================================= */

        [Fact]
        public void Audit_SP_GetLog_Works()
        {
            var count = ExecuteCount("sp_AuditGradeChanges_GetLog");
            Assert.True(count > 0);
        }
    }
}