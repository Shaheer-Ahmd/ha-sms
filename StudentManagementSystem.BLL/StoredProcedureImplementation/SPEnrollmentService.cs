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
            {
                var cmd = new SqlCommand("SELECT * FROM Enrollments", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(new Enrollment
                        {
                            EnrollmentID = (int)reader["EnrollmentID"],
                            StudentID = (int)reader["StudentID"],
                            OfferingID = (int)reader["OfferingID"],
                            Grade = reader["Grade"] == DBNull.Value ? null : reader["Grade"].ToString(),
                            EnrollmentDate = (DateTime)reader["EnrollmentDate"]
                        });
                    }
                }
            }
            return enrollments;
        }

        public Enrollment GetEnrollmentById(int enrollmentId, DateTime enrollmentDate)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Enrollments WHERE EnrollmentID = @EnrollmentID AND EnrollmentDate = @EnrollmentDate", conn);
                cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentId);
                cmd.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Enrollment
                        {
                            EnrollmentID = (int)reader["EnrollmentID"],
                            StudentID = (int)reader["StudentID"],
                            OfferingID = (int)reader["OfferingID"],
                            Grade = reader["Grade"] == DBNull.Value ? null : reader["Grade"].ToString(),
                            EnrollmentDate = (DateTime)reader["EnrollmentDate"]
                        };
                    }
                }
            }
            return null;
        }

        // Triggers AFTER UPDATE trigger (trg_After_GradeUpdate)
        public void UpdateEnrollment(Enrollment enrollment)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    UPDATE Enrollments 
                    SET Grade = @Grade
                    WHERE EnrollmentID = @EnrollmentID AND EnrollmentDate = @EnrollmentDate", conn);

                cmd.Parameters.AddWithValue("@EnrollmentID", enrollment.EnrollmentID);
                cmd.Parameters.AddWithValue("@EnrollmentDate", enrollment.EnrollmentDate);
                cmd.Parameters.AddWithValue("@Grade", (object)enrollment.Grade ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery(); // Trigger fires here
            }
        }

        // Uses sp_RegisterStudentForCourse stored procedure
        public void RegisterStudentForCourse(int studentId, int offeringId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("sp_RegisterStudentForCourse", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@OfferingID", offeringId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Enrollment> GetStudentEnrollments(int studentId)
        {
            var enrollments = new List<Enrollment>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Enrollments WHERE StudentID = @StudentID", conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(new Enrollment
                        {
                            EnrollmentID = (int)reader["EnrollmentID"],
                            StudentID = (int)reader["StudentID"],
                            OfferingID = (int)reader["OfferingID"],
                            Grade = reader["Grade"] == DBNull.Value ? null : reader["Grade"].ToString(),
                            EnrollmentDate = (DateTime)reader["EnrollmentDate"]
                        });
                    }
                }
            }
            return enrollments;
        }

        public List<Enrollment> GetCourseOfferingEnrollments(int offeringId)
        {
            var enrollments = new List<Enrollment>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Enrollments WHERE OfferingID = @OfferingID", conn);
                cmd.Parameters.AddWithValue("@OfferingID", offeringId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(new Enrollment
                        {
                            EnrollmentID = (int)reader["EnrollmentID"],
                            StudentID = (int)reader["StudentID"],
                            OfferingID = (int)reader["OfferingID"],
                            Grade = reader["Grade"] == DBNull.Value ? null : reader["Grade"].ToString(),
                            EnrollmentDate = (DateTime)reader["EnrollmentDate"]
                        });
                    }
                }
            }
            return enrollments;
        }

        // Uses filtered index IX_Enrollments_FailedCourses
        public List<Enrollment> GetFailingStudents()
        {
            var enrollments = new List<Enrollment>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Enrollments WHERE Grade = 'F'", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enrollments.Add(new Enrollment
                        {
                            EnrollmentID = (int)reader["EnrollmentID"],
                            StudentID = (int)reader["StudentID"],
                            OfferingID = (int)reader["OfferingID"],
                            Grade = reader["Grade"].ToString(),
                            EnrollmentDate = (DateTime)reader["EnrollmentDate"]
                        });
                    }
                }
            }
            return enrollments;
        }

        public List<CourseOffering> GetAllCourseOfferings()
        {
            var offerings = new List<CourseOffering>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM CourseOfferings", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        offerings.Add(new CourseOffering
                        {
                            OfferingID = (int)reader["OfferingID"],
                            CourseID = (int)reader["CourseID"],
                            SemesterID = (int)reader["SemesterID"],
                            MaxCapacity = (int)reader["MaxCapacity"],
                            CurrentEnrollment = (int)reader["CurrentEnrollment"]
                        });
                    }
                }
            }
            return offerings;
        }

        public CourseOffering GetCourseOfferingById(int offeringId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM CourseOfferings WHERE OfferingID = @OfferingID", conn);
                cmd.Parameters.AddWithValue("@OfferingID", offeringId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CourseOffering
                        {
                            OfferingID = (int)reader["OfferingID"],
                            CourseID = (int)reader["CourseID"],
                            SemesterID = (int)reader["SemesterID"],
                            MaxCapacity = (int)reader["MaxCapacity"],
                            CurrentEnrollment = (int)reader["CurrentEnrollment"]
                        };
                    }
                }
            }
            return null;
        }

        // Uses vw_AvailableCourseOfferings view
        public List<CourseOffering> GetAvailableCourseOfferings(int semesterId)
        {
            var offerings = new List<CourseOffering>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    SELECT * FROM vw_AvailableCourseOfferings 
                    WHERE SemesterID = @SemesterID", conn);
                cmd.Parameters.AddWithValue("@SemesterID", semesterId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        offerings.Add(new CourseOffering
                        {
                            OfferingID = (int)reader["OfferingID"],
                            CourseID = (int)reader["CourseID"],
                            SemesterID = (int)reader["SemesterID"],
                            MaxCapacity = (int)reader["MaxCapacity"],
                            CurrentEnrollment = (int)reader["CurrentEnrollment"]
                        });
                    }
                }
            }
            return offerings;
        }

        public void AddCourseOffering(CourseOffering offering)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO CourseOfferings (CourseID, SemesterID, MaxCapacity, CurrentEnrollment)
                    VALUES (@CourseID, @SemesterID, @MaxCapacity, @CurrentEnrollment)", conn);

                cmd.Parameters.AddWithValue("@CourseID", offering.CourseID);
                cmd.Parameters.AddWithValue("@SemesterID", offering.SemesterID);
                cmd.Parameters.AddWithValue("@MaxCapacity", offering.MaxCapacity);
                cmd.Parameters.AddWithValue("@CurrentEnrollment", offering.CurrentEnrollment);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCourseOffering(CourseOffering offering)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    UPDATE CourseOfferings 
                    SET CourseID = @CourseID, SemesterID = @SemesterID, 
                        MaxCapacity = @MaxCapacity, CurrentEnrollment = @CurrentEnrollment
                    WHERE OfferingID = @OfferingID", conn);

                cmd.Parameters.AddWithValue("@OfferingID", offering.OfferingID);
                cmd.Parameters.AddWithValue("@CourseID", offering.CourseID);
                cmd.Parameters.AddWithValue("@SemesterID", offering.SemesterID);
                cmd.Parameters.AddWithValue("@MaxCapacity", offering.MaxCapacity);
                cmd.Parameters.AddWithValue("@CurrentEnrollment", offering.CurrentEnrollment);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCourseOffering(int offeringId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM CourseOfferings WHERE OfferingID = @OfferingID", conn);
                cmd.Parameters.AddWithValue("@OfferingID", offeringId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<AuditGradeChange> GetGradeAuditLog()
        {
            var result = new List<AuditGradeChange>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
            SELECT TOP 1000
                a.AuditID,
                a.EnrollmentID,
                a.EnrollmentDate,
                a.OldGrade,
                a.NewGrade,
                a.ChangeDate
            FROM dbo.Audit_GradeChanges AS a
            ORDER BY a.ChangeDate DESC;";

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var audit = new AuditGradeChange
                        {
                            AuditID = (int)reader["AuditID"],
                            EnrollmentID = (int)reader["EnrollmentID"],
                            EnrollmentDate = (DateTime)reader["EnrollmentDate"],
                            OldGrade = reader["OldGrade"] as string,
                            NewGrade = reader["NewGrade"] as string,
                            ChangeDate = (DateTime)reader["ChangeDate"]
                        };

                        result.Add(audit);
                    }
                }
            }

            return result;
        }
    }
}
