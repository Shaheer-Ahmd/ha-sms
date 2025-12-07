using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.LinqImplementation
{
    public class LinqStudentService : IStudentService
    {
        private readonly string _connectionString;

        public LinqStudentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Student> GetAllStudents()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Students.ToList();
            }
        }

        public Student GetStudentById(int studentId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Students.FirstOrDefault(s => s.StudentID == studentId);
            }
        }

        public Student GetStudentByEmail(string email)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Students.FirstOrDefault(s => s.Email == email);
            }
        }

        public void AddStudent(Student student)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Students.Add(student);
                context.SaveChanges();
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                var existingStudent = context.Students.Find(student.StudentID);
                if (existingStudent != null)
                {
                    existingStudent.FirstName = student.FirstName;
                    existingStudent.LastName = student.LastName;
                    existingStudent.Email = student.Email;
                    existingStudent.DateOfBirth = student.DateOfBirth;
                    existingStudent.EnrollmentStatus = student.EnrollmentStatus;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Students 
                        SET EnrollmentStatus = 'Inactive' 
                        WHERE StudentID = @StudentID";
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Uses filtered index IX_Students_Active
        public List<Student> GetActiveStudents()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Students
                    .Where(s => s.EnrollmentStatus == "Active")
                    .ToList();
            }
        }

        public List<Student> SearchStudents(string searchTerm)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Students
                    .Where(s => s.FirstName.Contains(searchTerm) ||
                                s.LastName.Contains(searchTerm) ||
                                s.Email.Contains(searchTerm))
                    .ToList();
            }
        }

        // Uses fn_CalculateGPA function
        public decimal? GetStudentGPA(int studentId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                var sql = "SELECT dbo.fn_CalculateGPA(@p0) AS GPA";
                var result = context.Database.SqlQuery<decimal?>(sql, studentId).FirstOrDefault();
                return result;
            }
        }

        // Uses vw_StudentTranscript view
        public List<StudentTranscript> GetStudentTranscript(int studentId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.StudentTranscripts
                    .Where(st => st.StudentID == studentId)
                    .OrderBy(st => st.Year)
                    .ThenBy(st => st.Season)
                    .ToList();
            }
        }
    }
}
