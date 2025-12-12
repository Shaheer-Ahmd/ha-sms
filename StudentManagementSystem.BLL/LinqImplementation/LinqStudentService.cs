using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            using (var context = new StudentManagementContext(_connectionString))
            {
                var student = context.Students.Find(studentId);
                if (student != null)
                {
                    // Soft-delete behaviour preserved: mark as Inactive
                    student.EnrollmentStatus = "Inactive";
                    context.SaveChanges();
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

public decimal? GetStudentGPA(int studentId)
{
    using (var context = new StudentManagementContext(_connectionString))
    {
        // Join Enrollments -> CourseOfferings -> Courses
        var completed = (
            from e in context.Enrollments
            join o in context.CourseOfferings on e.OfferingID equals o.OfferingID
            join c in context.Courses on o.CourseID equals c.CourseID
            where e.StudentID == studentId && e.Grade != null
            select new
            {
                e.Grade,
                c.Credits
            })
            .ToList(); // materialize then compute in-memory (for grade mapping)

        if (!completed.Any())
            return null;

        decimal GradeToPoints(string grade)
        {
            // Adjust if you use +/-; this matches a simple A–F scale
            return grade switch
            {
                "A" => 4m,
                "B" => 3m,
                "C" => 2m,
                "D" => 1m,
                "F" => 0m,
                _   => 0m
            };
        }

        var totalCredits = completed.Sum(x => x.Credits);
        if (totalCredits == 0)
            return null;

        var totalPoints = completed.Sum(x => GradeToPoints(x.Grade!) * x.Credits);

        return totalPoints / totalCredits;
    }
}

        public List<StudentTranscript> GetStudentTranscript(int studentId)
{
    using (var context = new StudentManagementContext(_connectionString))
    {
        var query =
            from s in context.Students
            join e in context.Enrollments on s.StudentID equals e.StudentID
            join o in context.CourseOfferings on e.OfferingID equals o.OfferingID
            join c in context.Courses on o.CourseID equals c.CourseID
            join sem in context.Semesters on o.SemesterID equals sem.SemesterID
            where s.StudentID == studentId
            orderby sem.Year, sem.Season
            select new StudentTranscript
            {
                StudentID   = s.StudentID,
                FirstName   = s.FirstName,
                LastName    = s.LastName,
                CourseCode  = c.CourseCode,
                CourseTitle = c.Title,
                Credits     = c.Credits,
                Year        = sem.Year,
                Season      = sem.Season,
                Grade       = e.Grade
            };

        return query.ToList();
    }
}
    }
}
