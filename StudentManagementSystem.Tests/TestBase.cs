using Microsoft.Data.SqlClient;

namespace StudentManagementSystem.Tests
{
    public abstract class TestBase
    {
        protected SqlConnection Open()
        {
            var conn = new SqlConnection(
                // "Server=localhost,1433;" +
                "Server=127.0.0.1,1433;" +
                "Database=StudentManagementDB;" +
                "User Id=sa;" +
                "Password=YourPassword123!;" +
                "TrustServerCertificate=True;"
            );

            conn.Open();
            return conn;
        }
    }
}