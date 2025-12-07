using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.LinqImplementation
{
    public class LinqSemesterService : ISemesterService
    {
        private readonly string _connectionString;

        public LinqSemesterService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Semester> GetAllSemesters()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Semesters
                    .OrderByDescending(s => s.Year)
                    .ThenBy(s => s.Season)
                    .ToList();
            }
        }

        public Semester GetSemesterById(int semesterId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Semesters.FirstOrDefault(s => s.SemesterID == semesterId);
            }
        }

        public void AddSemester(Semester semester)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Semesters.Add(semester);
                context.SaveChanges();
            }
        }

        public void UpdateSemester(Semester semester)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Entry(semester).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteSemester(int semesterId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                var semester = context.Semesters.Find(semesterId);
                if (semester != null)
                {
                    context.Semesters.Remove(semester);
                    context.SaveChanges();
                }
            }
        }

        public Semester GetCurrentSemester()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Semesters
                    .OrderByDescending(s => s.Year)
                    .ThenByDescending(s => s.Season)
                    .FirstOrDefault();
            }
        }

        public List<Semester> GetRecentSemesters(int count)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Semesters
                    .OrderByDescending(s => s.Year)
                    .ThenByDescending(s => s.Season)
                    .Take(count)
                    .ToList();
            }
        }
    }
}
