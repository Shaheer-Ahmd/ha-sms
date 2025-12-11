using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.LinqImplementation
{
    public class LinqCourseOfferingService : ICourseOfferingService
    {
        private readonly string _connectionString;

        public LinqCourseOfferingService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CourseOffering> GetAllOfferings()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.CourseOfferings
                    .Include(co => co.Course)
                    .Include(co => co.Semester)
                    .ToList();
            }
        }

        public List<CourseOffering> GetOfferingsBySemester(int semesterId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.CourseOfferings
                    .Include(co => co.Course)
                    .Include(co => co.Semester)
                    .Where(co => co.SemesterID == semesterId)
                    .ToList();
            }
        }

        public List<CourseOffering> GetOfferingsByCourse(int courseId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.CourseOfferings
                    .Include(co => co.Course)
                    .Include(co => co.Semester)
                    .Where(co => co.CourseID == courseId)
                    .ToList();
            }
        }

        public List<CourseOffering> GetAvailableOfferings()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                // Uses filtered index IX_CourseOfferings_Available
                return context.CourseOfferings
                    .Include(co => co.Course)
                    .Include(co => co.Semester)
                    .Where(co => co.CurrentEnrollment < co.MaxCapacity)
                    .ToList();
            }
        }

        public CourseOffering GetOfferingById(int offeringId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.CourseOfferings
                    .Include(co => co.Course)
                    .Include(co => co.Semester)
                    .FirstOrDefault(co => co.OfferingID == offeringId);
            }
        }

        public void AddOffering(CourseOffering offering)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.CourseOfferings.Add(offering);
                context.SaveChanges();
            }
        }

        public void UpdateOffering(CourseOffering offering)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Entry(offering).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteOffering(int offeringId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                var offering = context.CourseOfferings.Find(offeringId);
                if (offering != null)
                {
                    context.CourseOfferings.Remove(offering);
                    context.SaveChanges();
                }
            }
        }
    }
}
