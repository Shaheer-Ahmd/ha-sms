using Xunit;
using Microsoft.Data.SqlClient;

namespace StudentManagementSystem.Tests
{
public class DepartmentHierarchyIntegrationTests : TestBase
{
    [Fact]
    public void Department_Hierarchy_Returns_Multiple_Levels()
    {
        using var c = Open();

        var cmd = new SqlCommand("EXEC dbo.sp_GetDepartmentHierarchy;", c);
        using var reader = cmd.ExecuteReader();

        int rows = 0;
        while (reader.Read()) rows++;

        Assert.True(rows > 1);
    }
}
}