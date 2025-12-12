using Xunit;
using Microsoft.Data.SqlClient;

namespace StudentManagementSystem.Tests
{
public class EnrollmentIntegrationTests : TestBase
{
    [Fact]
    public void Registration_Fails_When_Student_Has_Active_Hold()
    {
        using var c = Open();

        var cmd = new SqlCommand(@"
            DECLARE @student INT =
                (SELECT TOP 1 StudentID FROM Students WHERE StudentID IN
                    (SELECT StudentID FROM StudentHolds WHERE IsActive = 1));

            DECLARE @offering INT =
                (SELECT TOP 1 OfferingID FROM CourseOfferings);

            EXEC dbo.sp_RegisterStudentForCourse @student, @offering;
        ", c);

        Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
    }

    [Fact]
    public void Registration_Fails_When_Course_Is_Full()
    {
        using var c = Open();

        var cmd = new SqlCommand(@"
            DECLARE @offering INT =
                (SELECT TOP 1 OfferingID
                 FROM CourseOfferings
                 WHERE CurrentEnrollment = MaxCapacity);

            DECLARE @student INT =
                (SELECT TOP 1 StudentID FROM Students WHERE EnrollmentStatus = 'Active');

            EXEC dbo.sp_RegisterStudentForCourse @student, @offering;
        ", c);

        Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
    }
}
}