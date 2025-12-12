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
            using (var context = new StudentManagementContext(_connectionString))
            {
                var course = context.Courses.Find(courseId);
                if (course != null)
                {
                    context.Courses.Remove(course);
                    context.SaveChanges();
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

public bool CheckPrerequisitesMet(int studentId, int courseId)
{
    using (var context = new StudentManagementContext(_connectionString))
    {
        // 1) Get all direct prerequisite courses for this course
        var prerequisiteCourseIds = context.CoursePrerequisites
            .Where(cp => cp.CourseID == courseId)
            .Select(cp => cp.PrerequisiteCourseID)
            .ToList();

        // If no prereqs, trivially satisfied
        if (!prerequisiteCourseIds.Any())
            return true;

        var passingGrades = new[] { "A", "B", "C", "D" };

        // 2) Find which prerequisite courses the student has already passed
        var passedPrereqCourseIds =
            (from e in context.Enrollments
             join o in context.CourseOfferings on e.OfferingID equals o.OfferingID
             where e.StudentID == studentId
                   && e.Grade != null
                   && passingGrades.Contains(e.Grade)
                   && prerequisiteCourseIds.Contains(o.CourseID)
             select o.CourseID)
            .Distinct()
            .ToList();

        // 3) All prereqs must be in the "passed" set
        return prerequisiteCourseIds.All(id => passedPrereqCourseIds.Contains(id));
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

                // Already exists? nothing to do
                var exists = context.CoursePrerequisites.Any(cp =>
                    cp.CourseID == courseId &&
                    cp.PrerequisiteCourseID == prerequisiteCourseId);

                if (exists)
                    return;

                // --- Cycle check: does a path exist from prerequisiteCourseId -> courseId? ---
                // Load all existing edges once
                var allEdges = context.CoursePrerequisites
                    .Select(cp => new { cp.CourseID, cp.PrerequisiteCourseID })
                    .ToList();

                // Build lookup: for each course, which courses does it require?
                var lookup = allEdges
                    .GroupBy(e => e.CourseID)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.PrerequisiteCourseID).ToList()
                    );

                var toVisit = new Stack<int>();
                var visited = new HashSet<int>();

                toVisit.Push(prerequisiteCourseId);

                while (toVisit.Count > 0)
                {
                    var current = toVisit.Pop();
                    if (!visited.Add(current))
                        continue;

                    // If we can reach courseId from prerequisiteCourseId -> cycle
                    if (current == courseId)
                        throw new InvalidOperationException(
                            "This prerequisite would create a circular dependency between courses."
                        );

                    if (lookup.TryGetValue(current, out var nexts))
                    {
                        foreach (var next in nexts)
                        {
                            if (!visited.Contains(next))
                                toVisit.Push(next);
                        }
                    }
                }

                // --- Safe to insert ---
                var entity = new CoursePrerequisite
                {
                    CourseID = courseId,
                    PrerequisiteCourseID = prerequisiteCourseId
                };

                context.CoursePrerequisites.Add(entity);
                context.SaveChanges();
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
