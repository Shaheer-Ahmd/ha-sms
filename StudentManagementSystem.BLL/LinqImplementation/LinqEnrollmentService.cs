using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Data.SqlClient;
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
                try
                {
                    // 1) Load existing enrollment using composite key
                    var existing = context.Enrollments
                        .FirstOrDefault(e =>
                            e.EnrollmentID == enrollment.EnrollmentID &&
                            e.EnrollmentDate == enrollment.EnrollmentDate);

                    if (existing == null)
                    {
                        throw new InvalidOperationException(
                            $"Enrollment not found. ID={enrollment.EnrollmentID}, Date={enrollment.EnrollmentDate:d}");
                    }

                    // 2) Update only the fields you actually intend to change
                    existing.Grade = enrollment.Grade;

                    // 3) Save changes (trigger fires here)
                    context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Peel SQL Server error message for debugging
                    var innerMessage = ex.InnerException?.Message ?? "(no inner exception)";
                    var debugMessage =
                        "Error updating enrollment (EF DbUpdateException).\n" +
                        $"Outer: {ex.Message}\n" +
                        $"Inner: {innerMessage}";

                    // Re-throw with extra context so UI can show the real reason
                    throw new Exception(debugMessage, ex);
                }
                catch (Exception ex)
                {
                    // Catch-all for anything else
                    var debugMessage =
                        "Unexpected error updating enrollment.\n" +
                        $"Message: {ex.Message}";

                    throw new Exception(debugMessage, ex);
                }
            }
        }

        public void RegisterStudentForCourse(int studentId, int offeringId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                using var transaction = context.Database.BeginTransaction();

                try
                {
                    // 1. Check active holds
                    bool hasActiveHolds = context.StudentHolds
                        .Any(h => h.StudentID == studentId && h.IsActive);

                    throw new InvalidOperationException("Student has active holds. Registration denied.");
                    // if (hasActiveHolds)

                    // 2. Load offering and check capacity
                    var offering = context.CourseOfferings
                        .FirstOrDefault(o => o.OfferingID == offeringId);

                    if (offering == null)
                        throw new InvalidOperationException("Invalid OfferingID.");

                    if (offering.CurrentEnrollment >= offering.MaxCapacity)
                        throw new InvalidOperationException("Course is full.");

                    // Optional: prevent double registration in same offering
                    bool alreadyEnrolled = context.Enrollments
                        .Any(e => e.StudentID == studentId && e.OfferingID == offeringId);

                    if (alreadyEnrolled)
                        throw new InvalidOperationException("Student is already enrolled in this course offering.");

                    var courseId = offering.CourseID;

                    // 3. Check prerequisites via LINQ
                    var prerequisiteCourseIds = context.CoursePrerequisites
                        .Where(cp => cp.CourseID == courseId)
                        .Select(cp => cp.PrerequisiteCourseID)
                        .ToList();

                    if (prerequisiteCourseIds.Any())
                    {
                        var passingGrades = new[] { "A", "B", "C", "D" };

                        // Find which prerequisite courses this student has passed
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

                        bool prereqsOk = prerequisiteCourseIds.All(id => passedPrereqCourseIds.Contains(id));

                        if (!prereqsOk)
                            throw new InvalidOperationException("Prerequisites not satisfied.");
                    }

                    // 4. Atomic registration: insert enrollment + bump capacity
                    var enrollment = new Enrollment
                    {
                        StudentID = studentId,
                        OfferingID = offeringId,
                        Grade = null,
                        EnrollmentDate = DateTime.Today   // matches CONVERT(date, GETDATE())
                    };

                    context.Enrollments.Add(enrollment);
                    offering.CurrentEnrollment += 1;

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new InvalidOperationException($"Registration failed: {ex.Message}", ex);
                }
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
            using (var conn = new SqlConnection(_connectionString))
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
        public List<AuditGradeChange> GetGradeAuditLog()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.AuditGradeChanges
                    .OrderByDescending(a => a.ChangeDate)
                    .ThenByDescending(a => a.AuditID)
                    .ToList();
            }
        }

    }
}
