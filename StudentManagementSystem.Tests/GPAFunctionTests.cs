using Xunit;
using Microsoft.Data.SqlClient;

namespace StudentManagementSystem.Tests
{
public class GpaFunctionTests : TestBase
{
    [Fact]
    public void GPA_Is_Within_Valid_Range()
    {
        using var c = Open();

        var cmd = new SqlCommand(@"
            SELECT TOP 1 dbo.fn_CalculateGPA(StudentID)
            FROM Students;
        ", c);

        var result = cmd.ExecuteScalar();
        if (result != DBNull.Value)
        {
            var gpa = (decimal)result;
            Assert.InRange(gpa, 0.0m, 4.0m);
        }
    }
}
}