using System;
using System.Collections.Generic;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.Interfaces
{
    public interface IStudentService
    {
        // CRUD Operations
        List<Student> GetAllStudents();
        Student GetStudentById(int studentId);
        Student GetStudentByEmail(string email);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int studentId); // Uses INSTEAD OF trigger

        // Business logic operations
        List<Student> GetActiveStudents();
        List<Student> SearchStudents(string searchTerm);
        decimal? GetStudentGPA(int studentId); // Uses fn_CalculateGPA
        List<StudentTranscript> GetStudentTranscript(int studentId); // Uses view/TVF
    }
}
