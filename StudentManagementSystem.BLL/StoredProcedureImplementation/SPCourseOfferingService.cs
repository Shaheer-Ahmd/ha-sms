using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.StoredProcedureImplementation
{
    public class SPCourseOfferingService : ICourseOfferingService
    {
        private readonly string _connectionString;

        public SPCourseOfferingService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CourseOffering> GetAllOfferings()
        {
            var offerings = new List<CourseOffering>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                    SELECT co.OfferingID, co.CourseID, co.SemesterID, 
                           co.MaxCapacity, co.CurrentEnrollment,
                           c.CourseCode, c.Title, c.Credits,
                           s.Year, s.Season
                    FROM CourseOfferings co
                    INNER JOIN Courses c ON co.CourseID = c.CourseID
                    INNER JOIN Semesters s ON co.SemesterID = s.SemesterID", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            offerings.Add(new CourseOffering
                            {
                                OfferingID = reader.GetInt32(0),
                                CourseID = reader.GetInt32(1),
                                SemesterID = reader.GetInt32(2),
                                MaxCapacity = reader.GetInt32(3),
                                CurrentEnrollment = reader.GetInt32(4),
                                Course = new Course
                                {
                                    CourseID = reader.GetInt32(1),
                                    CourseCode = reader.GetString(5),
                                    Title = reader.GetString(6),
                                    Credits = reader.GetInt32(7)
                                },
                                Semester = new Semester
                                {
                                    SemesterID = reader.GetInt32(2),
                                    Year = reader.GetInt32(8),
                                    Season = reader.GetString(9)
                                }
                            });
                        }
                    }
                }
            }
            
            return offerings;
        }

        public List<CourseOffering> GetOfferingsBySemester(int semesterId)
        {
            var offerings = new List<CourseOffering>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                    SELECT co.OfferingID, co.CourseID, co.SemesterID, 
                           co.MaxCapacity, co.CurrentEnrollment,
                           c.CourseCode, c.Title
                    FROM CourseOfferings co
                    INNER JOIN Courses c ON co.CourseID = c.CourseID
                    WHERE co.SemesterID = @SemesterID", connection))
                {
                    command.Parameters.AddWithValue("@SemesterID", semesterId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            offerings.Add(new CourseOffering
                            {
                                OfferingID = reader.GetInt32(0),
                                CourseID = reader.GetInt32(1),
                                SemesterID = reader.GetInt32(2),
                                MaxCapacity = reader.GetInt32(3),
                                CurrentEnrollment = reader.GetInt32(4),
                                Course = new Course
                                {
                                    CourseID = reader.GetInt32(1),
                                    CourseCode = reader.GetString(5),
                                    Title = reader.GetString(6)
                                }
                            });
                        }
                    }
                }
            }
            
            return offerings;
        }

        public List<CourseOffering> GetOfferingsByCourse(int courseId)
        {
            var offerings = new List<CourseOffering>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                    SELECT co.OfferingID, co.CourseID, co.SemesterID, 
                           co.MaxCapacity, co.CurrentEnrollment,
                           s.Year, s.Season
                    FROM CourseOfferings co
                    INNER JOIN Semesters s ON co.SemesterID = s.SemesterID
                    WHERE co.CourseID = @CourseID", connection))
                {
                    command.Parameters.AddWithValue("@CourseID", courseId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            offerings.Add(new CourseOffering
                            {
                                OfferingID = reader.GetInt32(0),
                                CourseID = reader.GetInt32(1),
                                SemesterID = reader.GetInt32(2),
                                MaxCapacity = reader.GetInt32(3),
                                CurrentEnrollment = reader.GetInt32(4),
                                Semester = new Semester
                                {
                                    SemesterID = reader.GetInt32(2),
                                    Year = reader.GetInt32(5),
                                    Season = reader.GetString(6)
                                }
                            });
                        }
                    }
                }
            }
            
            return offerings;
        }

        public List<CourseOffering> GetAvailableOfferings()
        {
            var offerings = new List<CourseOffering>();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Uses vw_AvailableCourseOfferings view
                using (var command = new SqlCommand(@"
                    SELECT OfferingID, CourseID, SemesterID, 
                           MaxCapacity, CurrentEnrollment, SeatsRemaining
                    FROM vw_AvailableCourseOfferings", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            offerings.Add(new CourseOffering
                            {
                                OfferingID = reader.GetInt32(0),
                                CourseID = reader.GetInt32(1),
                                SemesterID = reader.GetInt32(2),
                                MaxCapacity = reader.GetInt32(3),
                                CurrentEnrollment = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            
            return offerings;
        }

        public CourseOffering GetOfferingById(int offeringId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                    SELECT OfferingID, CourseID, SemesterID, MaxCapacity, CurrentEnrollment
                    FROM CourseOfferings
                    WHERE OfferingID = @OfferingID", connection))
                {
                    command.Parameters.AddWithValue("@OfferingID", offeringId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CourseOffering
                            {
                                OfferingID = reader.GetInt32(0),
                                CourseID = reader.GetInt32(1),
                                SemesterID = reader.GetInt32(2),
                                MaxCapacity = reader.GetInt32(3),
                                CurrentEnrollment = reader.GetInt32(4)
                            };
                        }
                    }
                }
            }
            
            return null;
        }

        public void AddOffering(CourseOffering offering)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                    INSERT INTO CourseOfferings (CourseID, SemesterID, MaxCapacity, CurrentEnrollment)
                    VALUES (@CourseID, @SemesterID, @MaxCapacity, @CurrentEnrollment)", connection))
                {
                    command.Parameters.AddWithValue("@CourseID", offering.CourseID);
                    command.Parameters.AddWithValue("@SemesterID", offering.SemesterID);
                    command.Parameters.AddWithValue("@MaxCapacity", offering.MaxCapacity);
                    command.Parameters.AddWithValue("@CurrentEnrollment", offering.CurrentEnrollment);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOffering(CourseOffering offering)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                    UPDATE CourseOfferings
                    SET CourseID = @CourseID,
                        SemesterID = @SemesterID,
                        MaxCapacity = @MaxCapacity,
                        CurrentEnrollment = @CurrentEnrollment
                    WHERE OfferingID = @OfferingID", connection))
                {
                    command.Parameters.AddWithValue("@OfferingID", offering.OfferingID);
                    command.Parameters.AddWithValue("@CourseID", offering.CourseID);
                    command.Parameters.AddWithValue("@SemesterID", offering.SemesterID);
                    command.Parameters.AddWithValue("@MaxCapacity", offering.MaxCapacity);
                    command.Parameters.AddWithValue("@CurrentEnrollment", offering.CurrentEnrollment);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOffering(int offeringId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                    DELETE FROM CourseOfferings WHERE OfferingID = @OfferingID", connection))
                {
                    command.Parameters.AddWithValue("@OfferingID", offeringId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
