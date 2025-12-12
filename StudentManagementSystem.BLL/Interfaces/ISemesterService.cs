using System.Collections.Generic;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.Interfaces
{
    public interface ISemesterService
    {
        // CRUD Operations
        List<Semester> GetAllSemesters();
        Semester GetSemesterById(int semesterId);
        void AddSemester(Semester semester);
        void UpdateSemester(Semester semester);
        void DeleteSemester(int semesterId);

        // Business logic
        Semester GetCurrentSemester();
        List<Semester> GetRecentSemesters(int count);
    }
}
