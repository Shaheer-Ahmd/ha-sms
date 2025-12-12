using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.StoredProcedureImplementation
{
    public class SPStudentService : IStudentService
    {
        private readonly string _connectionString;

        public SPStudentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_GetAll", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(MapStudent(reader));
                    }
                }
            }

            return students;
        }

        public Student GetStudentById(int studentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_GetById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapStudent(reader);
                    }
                }
            }

            return null;
        }

        public Student GetStudentByEmail(string email)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_GetByEmail", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapStudent(reader);
                    }
                }
            }

            return null;
        }

        public void AddStudent(Student student)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_Add", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName",        student.FirstName);
                cmd.Parameters.AddWithValue("@LastName",         student.LastName);
                cmd.Parameters.AddWithValue("@Email",            student.Email);
                cmd.Parameters.AddWithValue("@EnrollmentStatus", student.EnrollmentStatus);
                cmd.Parameters.AddWithValue("@DateOfBirth",
                    (object?)student.DateOfBirth ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentID",        student.StudentID);
                cmd.Parameters.AddWithValue("@FirstName",        student.FirstName);
                cmd.Parameters.AddWithValue("@LastName",         student.LastName);
                cmd.Parameters.AddWithValue("@Email",            student.Email);
                cmd.Parameters.AddWithValue("@EnrollmentStatus", student.EnrollmentStatus);
                cmd.Parameters.AddWithValue("@DateOfBirth",
                    (object?)student.DateOfBirth ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteStudent(int studentId)
        {
            // DELETE triggers INSTEAD OF trigger, so behavior stays the same
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Student> GetActiveStudents()
        {
            var students = new List<Student>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_GetActive", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(MapStudent(reader));
                    }
                }
            }

            return students;
        }

        public List<Student> SearchStudents(string searchTerm)
        {
            var students = new List<Student>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_Search", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(MapStudent(reader));
                    }
                }
            }

            return students;
        }

        public decimal? GetStudentGPA(int studentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_GetGPA", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result == DBNull.Value ? (decimal?)null : Convert.ToDecimal(result);
            }
        }

        public List<StudentTranscript> GetStudentTranscript(int studentId)
        {
            var transcripts = new List<StudentTranscript>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Student_GetTranscript", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transcripts.Add(new StudentTranscript
                        {
                            StudentID   = (int)reader["StudentID"],
                            FirstName   = reader["FirstName"].ToString(),
                            LastName    = reader["LastName"].ToString(),
                            CourseCode  = reader["CourseCode"].ToString(),
                            CourseTitle = reader["CourseTitle"].ToString(),
                            Credits     = (int)reader["Credits"],
                            Year        = (int)reader["Year"],
                            Season      = reader["Season"].ToString(),
                            Grade       = reader["Grade"] == DBNull.Value
                                            ? null
                                            : reader["Grade"].ToString()
                        });
                    }
                }
            }

            return transcripts;
        }

        private Student MapStudent(IDataRecord reader)
        {
            return new Student
            {
                StudentID        = (int)reader["StudentID"],
                FirstName        = reader["FirstName"].ToString(),
                LastName         = reader["LastName"].ToString(),
                Email            = reader["Email"].ToString(),
                EnrollmentStatus = reader["EnrollmentStatus"].ToString(),
                DateOfBirth      = reader["DateOfBirth"] == DBNull.Value
                    ? (DateTime?)null
                    : (DateTime)reader["DateOfBirth"]
            };
        }
    }
}
