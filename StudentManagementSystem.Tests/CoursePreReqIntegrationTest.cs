using Xunit;
using Microsoft.Data.SqlClient;
using System;

namespace StudentManagementSystem.Tests
{
    public class CoursePrerequisiteIntegrationTests : TestBase
    {
        private int CreateTestCourse(SqlConnection c)
        {
            var cmd = new SqlCommand(@"
                INSERT INTO Courses (CourseCode, Title, Credits, DepartmentID)
                VALUES (@Code, 'Test Course', 3, 1);

                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ", c);

            cmd.Parameters.AddWithValue("@Code", Guid.NewGuid().ToString("N").Substring(0, 8));
            return (int)cmd.ExecuteScalar();
        }

        [Fact]
        public void Can_Add_Course_Prerequisite()
        {
            using var c = Open();

            int course = CreateTestCourse(c);
            int prereq = CreateTestCourse(c);

            var cmd = new SqlCommand(@"
                EXEC dbo.sp_CoursePrerequisite_Add @course, @prereq;

                SELECT COUNT(*)
                FROM CoursePrerequisites
                WHERE CourseID = @course
                  AND PrerequisiteCourseID = @prereq;
            ", c);

            cmd.Parameters.AddWithValue("@course", course);
            cmd.Parameters.AddWithValue("@prereq", prereq);

            int count = (int)cmd.ExecuteScalar();
            Assert.Equal(1, count);
        }

        [Fact]
        public void Cannot_Add_Self_Prerequisite()
        {
            using var c = Open();

            int course = CreateTestCourse(c);

            var cmd = new SqlCommand(@"
                EXEC dbo.sp_CoursePrerequisite_Add @course, @course;
            ", c);

            cmd.Parameters.AddWithValue("@course", course);

            Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
        }

        [Fact]
        public void Cannot_Add_Duplicate_Prerequisite()
        {
            using var c = Open();

            int course = CreateTestCourse(c);
            int prereq = CreateTestCourse(c);

            var cmd = new SqlCommand(@"
                EXEC dbo.sp_CoursePrerequisite_Add @course, @prereq;
                EXEC dbo.sp_CoursePrerequisite_Add @course, @prereq;

                SELECT COUNT(*)
                FROM CoursePrerequisites
                WHERE CourseID = @course
                  AND PrerequisiteCourseID = @prereq;
            ", c);

            cmd.Parameters.AddWithValue("@course", course);
            cmd.Parameters.AddWithValue("@prereq", prereq);

            int count = (int)cmd.ExecuteScalar();
            Assert.Equal(1, count);
        }

        [Fact]
        public void Cannot_Create_Circular_Prerequisite()
        {
            using var c = Open();

            int c1 = CreateTestCourse(c);
            int c2 = CreateTestCourse(c);

            var cmd = new SqlCommand(@"
                EXEC dbo.sp_CoursePrerequisite_Add @c1, @c2;
                EXEC dbo.sp_CoursePrerequisite_Add @c2, @c1;
            ", c);

            cmd.Parameters.AddWithValue("@c1", c1);
            cmd.Parameters.AddWithValue("@c2", c2);

            Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
        }
    }
}
