using System.Collections.Generic;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.Interfaces
{
    public interface ICourseService
    {
        // CRUD Operations
        List<Course> GetAllCourses();
        Course GetCourseById(int courseId);
        Course GetCourseByCourseCode(string courseCode);
        void AddCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(int courseId);

        // Business logic operations using views
        List<Course> GetCourseCatalog(); // Uses vw_CourseCatalog
        List<Course> GetCoursesByDepartment(int departmentId);
        List<CoursePrerequisite> GetCoursePrerequisites(int courseId);
        bool CheckPrerequisitesMet(int studentId, int courseId); // Uses fn_CheckPrerequisitesMet
        void AddCoursePrerequisite(int courseId, int prerequisiteCourseId);
        void RemoveCoursePrerequisite(int courseId, int prerequisiteCourseId);
        List<Course> GetAvailablePrerequisiteCourses(int courseId);
    }
}
