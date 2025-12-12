using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
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
            using (var cmd = new SqlCommand("sp_StudentHold_GetAll", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        holds.Add(MapHold(reader));
                    }
                }
            }

            return holds;
        }

        public StudentHold GetStudentHoldById(int holdId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_StudentHold_GetById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HoldID", holdId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapHold(reader);
                    }
                }
            }

            return null;
        }

        public void AddStudentHold(StudentHold hold)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_StudentHold_Add", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentID",   hold.StudentID);
                cmd.Parameters.AddWithValue("@HoldType",    hold.HoldType);
                cmd.Parameters.AddWithValue("@Reason",     (object?)hold.Reason ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateApplied", hold.DateApplied);
                cmd.Parameters.AddWithValue("@IsActive",    hold.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStudentHold(StudentHold hold)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_StudentHold_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HoldID",      hold.HoldID);
                cmd.Parameters.AddWithValue("@StudentID",   hold.StudentID);
                cmd.Parameters.AddWithValue("@HoldType",    hold.HoldType);
                cmd.Parameters.AddWithValue("@Reason",     (object?)hold.Reason ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateApplied", hold.DateApplied);
                cmd.Parameters.AddWithValue("@IsActive",    hold.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteStudentHold(int holdId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_StudentHold_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HoldID", holdId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<StudentHold> GetActiveHoldsForStudent(int studentId)
        {
            var holds = new List<StudentHold>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_StudentHold_GetActiveForStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        holds.Add(MapHold(reader));
                    }
                }
            }

            return holds;
        }

        public bool StudentHasActiveHolds(int studentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_StudentHold_HasActive", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null && Convert.ToInt32(result) == 1;
            }
        }

        private StudentHold MapHold(IDataRecord reader)
        {
            return new StudentHold
            {
                HoldID      = (int)reader["HoldID"],
                StudentID   = (int)reader["StudentID"],
                HoldType    = reader["HoldType"].ToString(),
                Reason      = reader["Reason"] == DBNull.Value
                                ? null
                                : reader["Reason"].ToString(),
                DateApplied = (DateTime)reader["DateApplied"],
                IsActive    = (bool)reader["IsActive"]
            };
        }
    }
}
