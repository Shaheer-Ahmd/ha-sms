using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
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
            using (var command = new SqlCommand("sp_CourseOffering_GetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        offerings.Add(new CourseOffering
                        {
                            OfferingID        = (int)reader["OfferingID"],
                            CourseID          = (int)reader["CourseID"],
                            SemesterID        = (int)reader["SemesterID"],
                            MaxCapacity       = (int)reader["MaxCapacity"],
                            CurrentEnrollment = (int)reader["CurrentEnrollment"],
                            Course = new Course
                            {
                                CourseID   = (int)reader["CourseID"],
                                CourseCode = reader["CourseCode"].ToString(),
                                Title      = reader["CourseTitle"].ToString(),
                                Credits    = (int)reader["Credits"]
                            },
                            Semester = new Semester
                            {
                                SemesterID = (int)reader["SemesterID"],
                                Year       = (int)reader["Year"],
                                Season     = reader["Season"].ToString()
                            }
                        });
                    }
                }
            }

            return offerings;
        }

        public List<CourseOffering> GetOfferingsBySemester(int semesterId)
        {
            var offerings = new List<CourseOffering>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CourseOffering_GetBySemester", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SemesterID", semesterId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        offerings.Add(new CourseOffering
                        {
                            OfferingID        = (int)reader["OfferingID"],
                            CourseID          = (int)reader["CourseID"],
                            SemesterID        = (int)reader["SemesterID"],
                            MaxCapacity       = (int)reader["MaxCapacity"],
                            CurrentEnrollment = (int)reader["CurrentEnrollment"],
                            Course = new Course
                            {
                                CourseID   = (int)reader["CourseID"],
                                CourseCode = reader["CourseCode"].ToString(),
                                Title      = reader["CourseTitle"].ToString()
                            }
                        });
                    }
                }
            }

            return offerings;
        }

        public List<CourseOffering> GetOfferingsByCourse(int courseId)
        {
            var offerings = new List<CourseOffering>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CourseOffering_GetByCourse", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CourseID", courseId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        offerings.Add(new CourseOffering
                        {
                            OfferingID        = (int)reader["OfferingID"],
                            CourseID          = (int)reader["CourseID"],
                            SemesterID        = (int)reader["SemesterID"],
                            MaxCapacity       = (int)reader["MaxCapacity"],
                            CurrentEnrollment = (int)reader["CurrentEnrollment"],
                            Semester = new Semester
                            {
                                SemesterID = (int)reader["SemesterID"],
                                Year       = (int)reader["Year"],
                                Season     = reader["Season"].ToString()
                            }
                        });
                    }
                }
            }

            return offerings;
        }

        public List<CourseOffering> GetAvailableOfferings()
        {
            var offerings = new List<CourseOffering>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CourseOffering_GetAvailable", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        offerings.Add(new CourseOffering
                        {
                            OfferingID        = (int)reader["OfferingID"],
                            CourseID          = (int)reader["CourseID"],
                            SemesterID        = (int)reader["SemesterID"],
                            MaxCapacity       = (int)reader["MaxCapacity"],
                            CurrentEnrollment = (int)reader["CurrentEnrollment"]
                            // SeatsRemaining is computed in SQL; you can expose it if you add a property
                        });
                    }
                }
            }

            return offerings;
        }

        public CourseOffering GetOfferingById(int offeringId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CourseOffering_GetById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OfferingID", offeringId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
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

            return null;
        }

        public void AddOffering(CourseOffering offering)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CourseOffering_Add", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CourseID",          offering.CourseID);
                command.Parameters.AddWithValue("@SemesterID",        offering.SemesterID);
                command.Parameters.AddWithValue("@MaxCapacity",       offering.MaxCapacity);
                command.Parameters.AddWithValue("@CurrentEnrollment", offering.CurrentEnrollment);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateOffering(CourseOffering offering)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CourseOffering_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OfferingID",        offering.OfferingID);
                command.Parameters.AddWithValue("@CourseID",          offering.CourseID);
                command.Parameters.AddWithValue("@SemesterID",        offering.SemesterID);
                command.Parameters.AddWithValue("@MaxCapacity",       offering.MaxCapacity);
                command.Parameters.AddWithValue("@CurrentEnrollment", offering.CurrentEnrollment);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteOffering(int offeringId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_CourseOffering_Delete", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OfferingID", offeringId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
