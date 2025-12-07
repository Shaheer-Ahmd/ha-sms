using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            {
                var cmd = new SqlCommand("SELECT * FROM Students ORDER BY LastName, FirstName", conn);
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
            {
                var cmd = new SqlCommand("SELECT * FROM Students WHERE StudentID = @StudentID", conn);
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
            {
                var cmd = new SqlCommand("SELECT * FROM Students WHERE Email = @Email", conn);
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
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO Students (FirstName, LastName, Email, EnrollmentStatus, DateOfBirth)
                    VALUES (@FirstName, @LastName, @Email, @EnrollmentStatus, @DateOfBirth)", conn);
                
                cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                cmd.Parameters.AddWithValue("@LastName", student.LastName);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@EnrollmentStatus", student.EnrollmentStatus);
                cmd.Parameters.AddWithValue("@DateOfBirth", (object)student.DateOfBirth ?? DBNull.Value);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    UPDATE Students 
                    SET FirstName = @FirstName, LastName = @LastName, 
                        Email = @Email, EnrollmentStatus = @EnrollmentStatus, 
                        DateOfBirth = @DateOfBirth
                    WHERE StudentID = @StudentID", conn);
                
                cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                cmd.Parameters.AddWithValue("@LastName", student.LastName);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@EnrollmentStatus", student.EnrollmentStatus);
                cmd.Parameters.AddWithValue("@DateOfBirth", (object)student.DateOfBirth ?? DBNull.Value);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Uses INSTEAD OF DELETE trigger
        public void DeleteStudent(int studentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM Students WHERE StudentID = @StudentID", conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Uses filtered index IX_Students_Active
        public List<Student> GetActiveStudents()
        {
            var students = new List<Student>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Students WHERE EnrollmentStatus = 'Active'", conn);
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
            {
                var cmd = new SqlCommand(@"
                    SELECT * FROM Students 
                    WHERE FirstName LIKE @SearchTerm OR LastName LIKE @SearchTerm OR Email LIKE @SearchTerm", conn);
                cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
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

        // Uses fn_CalculateGPA function
        public decimal? GetStudentGPA(int studentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT dbo.fn_CalculateGPA(@StudentID) AS GPA", conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result == DBNull.Value ? (decimal?)null : Convert.ToDecimal(result);
            }
        }

        // Uses vw_StudentTranscript view
        public List<StudentTranscript> GetStudentTranscript(int studentId)
        {
            var transcripts = new List<StudentTranscript>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    SELECT * FROM vw_StudentTranscript 
                    WHERE StudentID = @StudentID 
                    ORDER BY Year, Season", conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transcripts.Add(new StudentTranscript
                        {
                            StudentID = (int)reader["StudentID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            CourseCode = reader["CourseCode"].ToString(),
                            CourseTitle = reader["CourseTitle"].ToString(),
                            Credits = (int)reader["Credits"],
                            Year = (int)reader["Year"],
                            Season = reader["Season"].ToString(),
                            Grade = reader["Grade"] == DBNull.Value ? null : reader["Grade"].ToString()
                        });
                    }
                }
            }
            return transcripts;
        }

        private Student MapStudent(IDataReader reader)
        {
            return new Student
            {
                StudentID = (int)reader["StudentID"],
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Email = reader["Email"].ToString(),
                EnrollmentStatus = reader["EnrollmentStatus"].ToString(),
                DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["DateOfBirth"]
            };
        }
    }
}
