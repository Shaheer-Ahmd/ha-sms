using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.LinqImplementation
{
    public class LinqEnrollmentService : IEnrollmentService
    {
        private readonly string _connectionString;

        public LinqEnrollmentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Enrollment> GetAllEnrollments()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Enrollments
                    .Include(e => e.Student)
                    .Include(e => e.CourseOffering)
                    .ToList();
            }
        }

        public Enrollment GetEnrollmentById(int enrollmentId, DateTime enrollmentDate)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Enrollments
                    .Include(e => e.Student)
                    .Include(e => e.CourseOffering)
                    .FirstOrDefault(e => e.EnrollmentID == enrollmentId && e.EnrollmentDate == enrollmentDate);
            }
        }

        // Updates grade - triggers AFTER UPDATE trigger (trg_After_GradeUpdate)
        public void UpdateEnrollment(Enrollment enrollment)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Entry(enrollment).State = EntityState.Modified;
                context.SaveChanges(); // Trigger fires here
            }
        }

        // Uses sp_RegisterStudentForCourse stored procedure
        public void RegisterStudentForCourse(int studentId, int offeringId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Database.ExecuteSqlCommand(
                    "EXEC sp_RegisterStudentForCourse @p0, @p1",
                    studentId, offeringId
                );
            }
        }

        // Uses IX_Enrollments_StudentID index
        public List<Enrollment> GetStudentEnrollments(int studentId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Enrollments
                    .Include(e => e.CourseOffering.Course)
                    .Include(e => e.CourseOffering.Semester)
                    .Where(e => e.StudentID == studentId)
                    .OrderByDescending(e => e.EnrollmentDate)
                    .ToList();
            }
        }

        // Uses IX_Enrollments_OfferingID index
        public List<Enrollment> GetCourseOfferingEnrollments(int offeringId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Enrollments
                    .Include(e => e.Student)
                    .Where(e => e.OfferingID == offeringId)
                    .ToList();
            }
        }

        // Uses filtered index IX_Enrollments_FailedCourses
        public List<Enrollment> GetFailingStudents()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Enrollments
                    .Include(e => e.Student)
                    .Include(e => e.CourseOffering.Course)
                    .Where(e => e.Grade == "F")
                    .ToList();
            }
        }

        // Course Offering operations
        public List<CourseOffering> GetAllCourseOfferings()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.CourseOfferings
                    .Include(co => co.Course)
                    .Include(co => co.Semester)
                    .ToList();
            }
        }

        public CourseOffering GetCourseOfferingById(int offeringId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.CourseOfferings
                    .Include(co => co.Course)
                    .Include(co => co.Semester)
                    .FirstOrDefault(co => co.OfferingID == offeringId);
            }
        }

        // Uses vw_AvailableCourseOfferings view (simulated via LINQ)
        public List<CourseOffering> GetAvailableCourseOfferings(int semesterId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.CourseOfferings
                    .Include(co => co.Course)
                    .Include(co => co.Semester)
                    .Where(co => co.SemesterID == semesterId && co.CurrentEnrollment < co.MaxCapacity)
                    .ToList();
            }
        }

        public void AddCourseOffering(CourseOffering offering)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.CourseOfferings.Add(offering);
                context.SaveChanges();
            }
        }

        public void UpdateCourseOffering(CourseOffering offering)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Entry(offering).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCourseOffering(int offeringId)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM CourseOfferings WHERE OfferingID = @OfferingID";
                    cmd.Parameters.AddWithValue("@OfferingID", offeringId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
