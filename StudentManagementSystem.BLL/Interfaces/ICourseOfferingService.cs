using System.Collections.Generic;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.Interfaces
{
    public interface ICourseOfferingService
    {
        // CRUD Operations
        List<CourseOffering> GetAllOfferings();
        List<CourseOffering> GetOfferingsBySemester(int semesterId);
        List<CourseOffering> GetOfferingsByCourse(int courseId);
        List<CourseOffering> GetAvailableOfferings(); // Uses view
        CourseOffering GetOfferingById(int offeringId);
        void AddOffering(CourseOffering offering);
        void UpdateOffering(CourseOffering offering);
        void DeleteOffering(int offeringId);
    }
}
