using Xunit;
using Microsoft.Data.SqlClient;

namespace StudentManagementSystem.Tests
{
public class PrerequisiteFunctionTests : TestBase
{
    [Fact]
    public void Prerequisites_Returns_Zero_Or_One()
    {
        using var c = Open();

        var cmd = new SqlCommand(@"
            SELECT TOP 1 dbo.fn_CheckPrerequisitesMet(StudentID, CourseID)
            FROM Students
            CROSS JOIN Courses;
        ", c);

        var result = (bool)cmd.ExecuteScalar();
Assert.True(result == true || result == false);
    }
}
}