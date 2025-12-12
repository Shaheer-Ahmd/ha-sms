using Xunit;
using Microsoft.Data.SqlClient;

namespace StudentManagementSystem.Tests
{
    public class CoursePrerequisiteIntegrationTests : TestBase
    {
        [Fact]
        public void Can_Add_Course_Prerequisite()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                DECLARE @course INT = (SELECT MIN(CourseID) FROM Courses);
                DECLARE @prereq INT = @course + 1;

                EXEC dbo.sp_CoursePrerequisite_Add @course, @prereq;

                SELECT COUNT(*) 
                FROM CoursePrerequisites
                WHERE CourseID = @course
                  AND PrerequisiteCourseID = @prereq;
            ", c);

            int count = (int)cmd.ExecuteScalar();
            Assert.Equal(1, count);
        }

        [Fact]
        public void Cannot_Add_Self_Prerequisite()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                DECLARE @course INT = (SELECT MIN(CourseID) FROM Courses);
                EXEC dbo.sp_CoursePrerequisite_Add @course, @course;
            ", c);

            Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
        }

        [Fact]
        public void Cannot_Add_Duplicate_Prerequisite()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                DECLARE @course INT = (SELECT MIN(CourseID) FROM Courses);
                DECLARE @prereq INT = @course + 1;

                EXEC dbo.sp_CoursePrerequisite_Add @course, @prereq;
                EXEC dbo.sp_CoursePrerequisite_Add @course, @prereq;

                SELECT COUNT(*) 
                FROM CoursePrerequisites
                WHERE CourseID = @course
                  AND PrerequisiteCourseID = @prereq;
            ", c);

            int count = (int)cmd.ExecuteScalar();
            Assert.Equal(1, count); 
        }

        [Fact]
        public void Cannot_Create_Circular_Prerequisite()
        {
            using var c = Open();

            var cmd = new SqlCommand(@"
                DECLARE @c1 INT = (SELECT MIN(CourseID) FROM Courses);
                DECLARE @c2 INT = @c1 + 1;

                EXEC dbo.sp_CoursePrerequisite_Add @c1, @c2;
                EXEC dbo.sp_CoursePrerequisite_Add @c2, @c1;
            ", c);

            Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
        }
    }
}