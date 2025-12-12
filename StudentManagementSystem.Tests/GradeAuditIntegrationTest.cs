using Xunit;
using Microsoft.Data.SqlClient;

namespace StudentManagementSystem.Tests
{
public class GradeAuditIntegrationTests : TestBase
{
    [Fact]
    public void Updating_Grade_Creates_Audit_Record()
    {
        using var c = Open();

        var cmd = new SqlCommand(@"
            DECLARE @id INT, @date DATE;

            SELECT TOP 1
                @id = EnrollmentID,
                @date = EnrollmentDate
            FROM Enrollments
            WHERE Grade IS NOT NULL;

            UPDATE Enrollments
            SET Grade = 'A'
            WHERE EnrollmentID = @id AND EnrollmentDate = @date;

            SELECT COUNT(*) FROM Audit_GradeChanges
            WHERE EnrollmentID = @id AND EnrollmentDate = @date;
        ", c);

        var count = (int)cmd.ExecuteScalar();
        Assert.True(count > 0);
    }
}
}