using System;
using System.Collections.Generic;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.Interfaces
{
    public interface IEnrollmentService
    {
        List<Enrollment> GetAllEnrollments();
        Enrollment GetEnrollmentById(int enrollmentId, DateTime enrollmentDate);
        void UpdateEnrollment(Enrollment enrollment);

        void RegisterStudentForCourse(int studentId, int offeringId);
        List<Enrollment> GetStudentEnrollments(int studentId);
        List<Enrollment> GetCourseOfferingEnrollments(int offeringId);
        List<Enrollment> GetFailingStudents();

        List<CourseOffering> GetAllCourseOfferings();
        CourseOffering GetCourseOfferingById(int offeringId);
        List<CourseOffering> GetAvailableCourseOfferings(int semesterId); // Uses vw_AvailableCourseOfferings
        void AddCourseOffering(CourseOffering offering);
        void UpdateCourseOffering(CourseOffering offering);
        void DeleteCourseOffering(int offeringId);
        List<AuditGradeChange> GetGradeAuditLog();

    }
}
