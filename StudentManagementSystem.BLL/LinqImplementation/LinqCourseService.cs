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
    public class LinqCourseService : ICourseService
    {
        private readonly string _connectionString;

        public LinqCourseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Course> GetAllCourses()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Courses.Include(c => c.Department).ToList();
            }
        }

        public Course GetCourseById(int courseId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Courses
                    .Include(c => c.Department)
                    .FirstOrDefault(c => c.CourseID == courseId);
            }
        }

        public Course GetCourseByCourseCode(string courseCode)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Courses
                    .Include(c => c.Department)
                    .FirstOrDefault(c => c.CourseCode == courseCode);
            }
        }

        public void AddCourse(Course course)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Courses.Add(course);
                context.SaveChanges();
            }
        }

        public void UpdateCourse(Course course)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                var existingCourse = context.Courses.Find(course.CourseID);
                if (existingCourse != null)
                {
                    existingCourse.CourseCode = course.CourseCode;
                    existingCourse.Title = course.Title;
                    existingCourse.Description = course.Description;
                    existingCourse.Credits = course.Credits;
                    existingCourse.DepartmentID = course.DepartmentID;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteCourse(int courseId)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Courses WHERE CourseID = @CourseID";
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Uses vw_CourseCatalog view (simulated via LINQ)
        public List<Course> GetCourseCatalog()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Courses
                    .Include(c => c.Department)
                    .Where(c => c.Department.IsActive)
                    .OrderBy(c => c.CourseCode)
                    .ToList();
            }
        }

        // Uses IX_Courses_DepartmentID index
        public List<Course> GetCoursesByDepartment(int departmentId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Courses
                    .Where(c => c.DepartmentID == departmentId)
                    .OrderBy(c => c.CourseCode)
                    .ToList();
            }
        }

        public List<CoursePrerequisite> GetCoursePrerequisites(int courseId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.CoursePrerequisites
                    .Include(cp => cp.PrerequisiteCourse)
                    .Where(cp => cp.CourseID == courseId)
                    .ToList();
            }
        }

        // Uses fn_CheckPrerequisitesMet function
        public bool CheckPrerequisitesMet(int studentId, int courseId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                var sql = "SELECT dbo.fn_CheckPrerequisitesMet(@p0, @p1) AS Result";
                var result = context.Database.SqlQuery<bool>(sql, studentId, courseId).FirstOrDefault();
                return result;
            }
        }
        public List<Course> GetAvailablePrerequisiteCourses(int courseId)
    {
        using (var context = new StudentManagementContext(_connectionString))
        {
            var existingPrereqIds = context.CoursePrerequisites
                .Where(cp => cp.CourseID == courseId)
                .Select(cp => cp.PrerequisiteCourseID)
                .ToList();

            // Exclude the course itself + already-added prerequisites
            return context.Courses
                .Where(c => c.CourseID != courseId && !existingPrereqIds.Contains(c.CourseID))
                .OrderBy(c => c.CourseCode)
                .ToList();
        }
    }

    public void AddCoursePrerequisite(int courseId, int prerequisiteCourseId)
    {
        using (var context = new StudentManagementContext(_connectionString))
        {
            if (courseId == prerequisiteCourseId)
                throw new InvalidOperationException("A course cannot be its own prerequisite.");

            var exists = context.CoursePrerequisites.Any(cp =>
                cp.CourseID == courseId && cp.PrerequisiteCourseID == prerequisiteCourseId);

            if (!exists)
            {
                var entity = new CoursePrerequisite
                {
                    CourseID = courseId,
                    PrerequisiteCourseID = prerequisiteCourseId
                };
                context.CoursePrerequisites.Add(entity);
                context.SaveChanges();
            }
        }
    }

    public void RemoveCoursePrerequisite(int courseId, int prerequisiteCourseId)
    {
        using (var context = new StudentManagementContext(_connectionString))
        {
            var entity = context.CoursePrerequisites.SingleOrDefault(cp =>
                cp.CourseID == courseId && cp.PrerequisiteCourseID == prerequisiteCourseId);

            if (entity != null)
            {
                context.CoursePrerequisites.Remove(entity);
                context.SaveChanges();
            }
        }
    }
    }
}
