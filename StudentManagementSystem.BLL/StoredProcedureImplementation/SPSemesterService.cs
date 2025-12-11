using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.StoredProcedureImplementation
{
    public class SPSemesterService : ISemesterService
    {
        private readonly string _connectionString;

        public SPSemesterService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Semester> GetAllSemesters()
        {
            var semesters = new List<Semester>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Semesters ORDER BY Year DESC, Season", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        semesters.Add(new Semester
                        {
                            SemesterID = (int)reader["SemesterID"],
                            Year = (int)reader["Year"],
                            Season = reader["Season"].ToString()
                        });
                    }
                }
            }
            return semesters;
        }

        public Semester GetSemesterById(int semesterId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Semesters WHERE SemesterID = @SemesterID", conn);
                cmd.Parameters.AddWithValue("@SemesterID", semesterId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Semester
                        {
                            SemesterID = (int)reader["SemesterID"],
                            Year = (int)reader["Year"],
                            Season = reader["Season"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void AddSemester(Semester semester)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("INSERT INTO Semesters (Year, Season) VALUES (@Year, @Season)", conn);
                cmd.Parameters.AddWithValue("@Year", semester.Year);
                cmd.Parameters.AddWithValue("@Season", semester.Season);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSemester(Semester semester)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("UPDATE Semesters SET Year = @Year, Season = @Season WHERE SemesterID = @SemesterID", conn);
                cmd.Parameters.AddWithValue("@SemesterID", semester.SemesterID);
                cmd.Parameters.AddWithValue("@Year", semester.Year);
                cmd.Parameters.AddWithValue("@Season", semester.Season);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteSemester(int semesterId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM Semesters WHERE SemesterID = @SemesterID", conn);
                cmd.Parameters.AddWithValue("@SemesterID", semesterId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Semester GetCurrentSemester()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT TOP 1 * FROM Semesters ORDER BY Year DESC, Season DESC", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Semester
                        {
                            SemesterID = (int)reader["SemesterID"],
                            Year = (int)reader["Year"],
                            Season = reader["Season"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public List<Semester> GetRecentSemesters(int count)
        {
            var semesters = new List<Semester>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand($"SELECT TOP {count} * FROM Semesters ORDER BY Year DESC, Season DESC", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        semesters.Add(new Semester
                        {
                            SemesterID = (int)reader["SemesterID"],
                            Year = (int)reader["Year"],
                            Season = reader["Season"].ToString()
                        });
                    }
                }
            }
            return semesters;
        }
    }
}
