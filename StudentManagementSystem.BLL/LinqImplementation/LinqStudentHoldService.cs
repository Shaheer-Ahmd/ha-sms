using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.LinqImplementation
{
    public class LinqStudentHoldService : IStudentHoldService
    {
        private readonly string _connectionString;

        public LinqStudentHoldService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<StudentHold> GetAllStudentHolds()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.StudentHolds
                    .Include(sh => sh.Student)
                    .ToList();
            }
        }

        public StudentHold GetStudentHoldById(int holdId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.StudentHolds
                    .Include(sh => sh.Student)
                    .FirstOrDefault(sh => sh.HoldID == holdId);
            }
        }

        public void AddStudentHold(StudentHold hold)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.StudentHolds.Add(hold);
                context.SaveChanges();
            }
        }

        public void UpdateStudentHold(StudentHold hold)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Entry(hold).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteStudentHold(int holdId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                var hold = context.StudentHolds.Find(holdId);
                if (hold != null)
                {
                    context.StudentHolds.Remove(hold);
                    context.SaveChanges();
                }
            }
        }

        // Uses filtered index IX_StudentHolds_Student_IsActive
        public List<StudentHold> GetActiveHoldsForStudent(int studentId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.StudentHolds
                    .Where(sh => sh.StudentID == studentId && sh.IsActive)
                    .ToList();
            }
        }

        public bool StudentHasActiveHolds(int studentId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.StudentHolds
                    .Any(sh => sh.StudentID == studentId && sh.IsActive);
            }
        }
    }
}
