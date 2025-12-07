using System.Collections.Generic;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.Interfaces
{
    public interface IStudentHoldService
    {
        // CRUD Operations
        List<StudentHold> GetAllStudentHolds();
        StudentHold GetStudentHoldById(int holdId);
        void AddStudentHold(StudentHold hold);
        void UpdateStudentHold(StudentHold hold);
        void DeleteStudentHold(int holdId);
        
        // Business logic using indexes
        List<StudentHold> GetActiveHoldsForStudent(int studentId); // Uses filtered index
        bool StudentHasActiveHolds(int studentId);
    }
}
