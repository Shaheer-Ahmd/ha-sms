using System.Collections.Generic;
using System.Data;
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
            using (var cmd = new SqlCommand("sp_Semester_GetAll", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        semesters.Add(MapSemester(reader));
                    }
                }
            }

            return semesters;
        }

        public Semester GetSemesterById(int semesterId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Semester_GetById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SemesterID", semesterId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapSemester(reader);
                    }
                }
            }

            return null;
        }

        public void AddSemester(Semester semester)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Semester_Add", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Year",   semester.Year);
                cmd.Parameters.AddWithValue("@Season", semester.Season);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSemester(Semester semester)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Semester_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SemesterID", semester.SemesterID);
                cmd.Parameters.AddWithValue("@Year",       semester.Year);
                cmd.Parameters.AddWithValue("@Season",     semester.Season);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteSemester(int semesterId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Semester_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SemesterID", semesterId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Semester GetCurrentSemester()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Semester_GetCurrent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapSemester(reader);
                    }
                }
            }

            return null;
        }

        public List<Semester> GetRecentSemesters(int count)
        {
            var semesters = new List<Semester>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Semester_GetRecent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Count", count);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        semesters.Add(MapSemester(reader));
                    }
                }
            }

            return semesters;
        }

        private Semester MapSemester(IDataRecord reader)
        {
            return new Semester
            {
                SemesterID = (int)reader["SemesterID"],
                Year       = (int)reader["Year"],
                Season     = reader["Season"].ToString()
            };
        }
    }
}
