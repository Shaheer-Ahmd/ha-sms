using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.StoredProcedureImplementation
{
    public class SPStudentHoldService : IStudentHoldService
    {
        private readonly string _connectionString;

        public SPStudentHoldService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<StudentHold> GetAllStudentHolds()
        {
            var holds = new List<StudentHold>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM StudentHolds", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        holds.Add(new StudentHold
                        {
                            HoldID = (int)reader["HoldID"],
                            StudentID = (int)reader["StudentID"],
                            HoldType = reader["HoldType"].ToString(),
                            Reason = reader["Reason"] == DBNull.Value ? null : reader["Reason"].ToString(),
                            DateApplied = (DateTime)reader["DateApplied"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return holds;
        }

        public StudentHold GetStudentHoldById(int holdId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM StudentHolds WHERE HoldID = @HoldID", conn);
                cmd.Parameters.AddWithValue("@HoldID", holdId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new StudentHold
                        {
                            HoldID = (int)reader["HoldID"],
                            StudentID = (int)reader["StudentID"],
                            HoldType = reader["HoldType"].ToString(),
                            Reason = reader["Reason"] == DBNull.Value ? null : reader["Reason"].ToString(),
                            DateApplied = (DateTime)reader["DateApplied"],
                            IsActive = (bool)reader["IsActive"]
                        };
                    }
                }
            }
            return null;
        }

        public void AddStudentHold(StudentHold hold)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO StudentHolds (StudentID, HoldType, Reason, DateApplied, IsActive)
                    VALUES (@StudentID, @HoldType, @Reason, @DateApplied, @IsActive)", conn);
                
                cmd.Parameters.AddWithValue("@StudentID", hold.StudentID);
                cmd.Parameters.AddWithValue("@HoldType", hold.HoldType);
                cmd.Parameters.AddWithValue("@Reason", (object)hold.Reason ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateApplied", hold.DateApplied);
                cmd.Parameters.AddWithValue("@IsActive", hold.IsActive);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStudentHold(StudentHold hold)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    UPDATE StudentHolds 
                    SET StudentID = @StudentID, HoldType = @HoldType, Reason = @Reason, 
                        DateApplied = @DateApplied, IsActive = @IsActive
                    WHERE HoldID = @HoldID", conn);
                
                cmd.Parameters.AddWithValue("@HoldID", hold.HoldID);
                cmd.Parameters.AddWithValue("@StudentID", hold.StudentID);
                cmd.Parameters.AddWithValue("@HoldType", hold.HoldType);
                cmd.Parameters.AddWithValue("@Reason", (object)hold.Reason ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateApplied", hold.DateApplied);
                cmd.Parameters.AddWithValue("@IsActive", hold.IsActive);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteStudentHold(int holdId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM StudentHolds WHERE HoldID = @HoldID", conn);
                cmd.Parameters.AddWithValue("@HoldID", holdId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Uses filtered index IX_StudentHolds_Student_IsActive
        public List<StudentHold> GetActiveHoldsForStudent(int studentId)
        {
            var holds = new List<StudentHold>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM StudentHolds WHERE StudentID = @StudentID AND IsActive = 1", conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        holds.Add(new StudentHold
                        {
                            HoldID = (int)reader["HoldID"],
                            StudentID = (int)reader["StudentID"],
                            HoldType = reader["HoldType"].ToString(),
                            Reason = reader["Reason"] == DBNull.Value ? null : reader["Reason"].ToString(),
                            DateApplied = (DateTime)reader["DateApplied"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return holds;
        }

        public bool StudentHasActiveHolds(int studentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT COUNT(*) FROM StudentHolds WHERE StudentID = @StudentID AND IsActive = 1", conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                conn.Open();
                var count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
