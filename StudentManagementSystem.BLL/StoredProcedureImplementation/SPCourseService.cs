using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.StoredProcedureImplementation
{
    public class SPCourseService : ICourseService
    {
        private readonly string _connectionString;

        public SPCourseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Course> GetAllCourses()
        {
            var courses = new List<Course>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_GetAll", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(MapCourse(reader));
                    }
                }
            }

            return courses;
        }

        public Course GetCourseById(int courseId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_GetById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCourse(reader);
                    }
                }
            }

            return null;
        }

        public Course GetCourseByCourseCode(string courseCode)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_GetByCode", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseCode", courseCode);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCourse(reader);
                    }
                }
            }

            return null;
        }

        public void AddCourse(Course course)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_Add", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CourseCode",   course.CourseCode);
                cmd.Parameters.AddWithValue("@Title",        course.Title);
                cmd.Parameters.AddWithValue("@Description", (object?)course.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Credits",      course.Credits);
                cmd.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCourse(Course course)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CourseID",     course.CourseID);
                cmd.Parameters.AddWithValue("@CourseCode",   course.CourseCode);
                cmd.Parameters.AddWithValue("@Title",        course.Title);
                cmd.Parameters.AddWithValue("@Description", (object?)course.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Credits",      course.Credits);
                cmd.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCourse(int courseId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Course> GetCourseCatalog()
        {
            var courses = new List<Course>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_GetCatalog", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(new Course
                        {
                            CourseID   = (int)reader["CourseID"],
                            CourseCode = reader["CourseCode"].ToString(),
                            Title      = reader["CourseTitle"].ToString(),
                            Credits    = (int)reader["Credits"],
                            Department = new Department
                            {
                                DepartmentID   = (int)reader["DepartmentID"],
                                DepartmentName = reader["DepartmentName"].ToString()
                            }
                        });
                    }
                }
            }

            return courses;
        }

        public List<Course> GetCoursesByDepartment(int departmentId)
        {
            var courses = new List<Course>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_GetByDepartment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentID", departmentId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(MapCourse(reader));
                    }
                }
            }

            return courses;
        }

        public List<CoursePrerequisite> GetCoursePrerequisites(int courseId)
        {
            var prerequisites = new List<CoursePrerequisite>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_CoursePrerequisite_GetByCourse", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prerequisites.Add(new CoursePrerequisite
                        {
                            CourseID             = (int)reader["CourseID"],
                            PrerequisiteCourseID = (int)reader["PrerequisiteCourseID"],
                            PrerequisiteCourse   = new Course
                            {
                                CourseID   = (int)reader["PrerequisiteCourseID"],
                                CourseCode = reader["CourseCode"].ToString(),
                                Title      = reader["Title"].ToString()
                            }
                        });
                    }
                }
            }

            return prerequisites;
        }

        public bool CheckPrerequisitesMet(int studentId, int courseId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_CheckPrerequisitesMet", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@CourseID",  courseId);

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null && Convert.ToBoolean(result);
            }
        }

        public List<Course> GetAvailablePrerequisiteCourses(int courseId)
        {
            var courses = new List<Course>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Course_GetAvailablePrerequisiteCourses", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(MapCourse(reader));
                    }
                }
            }

            return courses;
        }

        public void AddCoursePrerequisite(int courseId, int prerequisiteCourseId)
        {
            // quick guard in C# (also enforced in SP)
            if (courseId == prerequisiteCourseId)
                throw new InvalidOperationException("A course cannot be its own prerequisite.");

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_CoursePrerequisite_Add", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                cmd.Parameters.AddWithValue("@PrereqID", prerequisiteCourseId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveCoursePrerequisite(int courseId, int prerequisiteCourseId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_CoursePrerequisite_Remove", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                cmd.Parameters.AddWithValue("@PrereqID", prerequisiteCourseId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private Course MapCourse(IDataRecord reader)
        {
            return new Course
            {
                CourseID     = (int)reader["CourseID"],
                CourseCode   = reader["CourseCode"].ToString(),
                Title        = reader["Title"].ToString(),
                Description  = reader["Description"] == DBNull.Value
                                    ? null
                                    : reader["Description"].ToString(),
                Credits      = (int)reader["Credits"],
                DepartmentID = (int)reader["DepartmentID"],
                Department   = new Department
                {
                    DepartmentID   = (int)reader["DepartmentID"],
                    DepartmentName = reader["DepartmentName"].ToString()
                }
            };
        }
    }
}
