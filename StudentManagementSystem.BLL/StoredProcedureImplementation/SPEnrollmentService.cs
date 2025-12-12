using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.StoredProcedureImplementation
{
    public class SPEnrollmentService : IEnrollmentService
    {
        private readonly string _connectionString;

        public SPEnrollmentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Enrollment> GetAllEnrollments()
        {
            var enrollments = new List<Enrollment>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_GetAll", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(MapEnrollment(reader));
                    }
                }
            }

            return enrollments;
        }

        public Enrollment GetEnrollmentById(int enrollmentId, DateTime enrollmentDate)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_GetById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EnrollmentID",   enrollmentId);
                cmd.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapEnrollment(reader);
                    }
                }
            }

            return null;
        }

        public void UpdateEnrollment(Enrollment enrollment)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_UpdateGrade", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EnrollmentID",   enrollment.EnrollmentID);
                cmd.Parameters.AddWithValue("@EnrollmentDate", enrollment.EnrollmentDate);
                cmd.Parameters.AddWithValue("@Grade",
                    (object?)enrollment.Grade ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery(); // trigger fires here
            }
        }

        public void RegisterStudentForCourse(int studentId, int offeringId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_RegisterStudentForCourse", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID",  studentId);
                cmd.Parameters.AddWithValue("@OfferingID", offeringId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Enrollment> GetStudentEnrollments(int studentId)
        {
            var enrollments = new List<Enrollment>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_GetByStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(MapEnrollment(reader));
                    }
                }
            }

            return enrollments;
        }

        public List<Enrollment> GetCourseOfferingEnrollments(int offeringId)
        {
            var enrollments = new List<Enrollment>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_GetByOffering", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OfferingID", offeringId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(MapEnrollment(reader));
                    }
                }
            }

            return enrollments;
        }

        public List<Enrollment> GetFailingStudents()
        {
            var enrollments = new List<Enrollment>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_GetFailing", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(MapEnrollment(reader));
                    }
                }
            }

            return enrollments;
        }

        public List<CourseOffering> GetAllCourseOfferings()
        {
            var offerings = new List<CourseOffering>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_GetAllCourseOfferings", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        offerings.Add(MapCourseOffering(reader));
                    }
                }
            }

            return offerings;
        }

        public CourseOffering GetCourseOfferingById(int offeringId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_GetCourseOfferingById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OfferingID", offeringId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCourseOffering(reader);
                    }
                }
            }

            return null;
        }

        public List<CourseOffering> GetAvailableCourseOfferings(int semesterId)
        {
            var offerings = new List<CourseOffering>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_GetAvailableCourseOfferings", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SemesterID", semesterId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        offerings.Add(MapCourseOffering(reader));
                    }
                }
            }

            return offerings;
        }

        public void AddCourseOffering(CourseOffering offering)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_AddCourseOffering", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CourseID",          offering.CourseID);
                cmd.Parameters.AddWithValue("@SemesterID",        offering.SemesterID);
                cmd.Parameters.AddWithValue("@MaxCapacity",       offering.MaxCapacity);
                cmd.Parameters.AddWithValue("@CurrentEnrollment", offering.CurrentEnrollment);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCourseOffering(CourseOffering offering)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_UpdateCourseOffering", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OfferingID",        offering.OfferingID);
                cmd.Parameters.AddWithValue("@CourseID",          offering.CourseID);
                cmd.Parameters.AddWithValue("@SemesterID",        offering.SemesterID);
                cmd.Parameters.AddWithValue("@MaxCapacity",       offering.MaxCapacity);
                cmd.Parameters.AddWithValue("@CurrentEnrollment", offering.CurrentEnrollment);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCourseOffering(int offeringId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Enrollment_DeleteCourseOffering", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OfferingID", offeringId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<AuditGradeChange> GetGradeAuditLog()
        {
            var result = new List<AuditGradeChange>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_AuditGradeChanges_GetLog", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new AuditGradeChange
                        {
                            AuditID        = (int)reader["AuditID"],
                            EnrollmentID   = (int)reader["EnrollmentID"],
                            EnrollmentDate = (DateTime)reader["EnrollmentDate"],
                            OldGrade       = reader["OldGrade"] as string,
                            NewGrade       = reader["NewGrade"] as string,
                            ChangeDate     = (DateTime)reader["ChangeDate"]
                        });
                    }
                }
            }

            return result;
        }

        private Enrollment MapEnrollment(IDataRecord reader)
        {
            return new Enrollment
            {
                EnrollmentID   = (int)reader["EnrollmentID"],
                StudentID      = (int)reader["StudentID"],
                OfferingID     = (int)reader["OfferingID"],
                Grade          = reader["Grade"] == DBNull.Value
                                    ? null
                                    : reader["Grade"].ToString(),
                EnrollmentDate = (DateTime)reader["EnrollmentDate"]
            };
        }

        private CourseOffering MapCourseOffering(IDataRecord reader)
        {
            return new CourseOffering
            {
                OfferingID        = (int)reader["OfferingID"],
                CourseID          = (int)reader["CourseID"],
                SemesterID        = (int)reader["SemesterID"],
                MaxCapacity       = (int)reader["MaxCapacity"],
                CurrentEnrollment = (int)reader["CurrentEnrollment"]
            };
        }
    }
}
