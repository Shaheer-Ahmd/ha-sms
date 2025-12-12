using Xunit;
using Microsoft.Data.SqlClient;

namespace StudentManagementSystem.Tests
{
public class TranscriptIntegrationTests : TestBase
{
    [Fact]
    public void Transcript_Returns_Only_Completed_Courses()
    {
        using var c = Open();

        var cmd = new SqlCommand(@"
            SELECT COUNT(*) 
            FROM vw_StudentTranscript
            WHERE Grade IS NULL;
        ", c);

        var count = (int)cmd.ExecuteScalar();
        Assert.Equal(0, count);
    }
}
}