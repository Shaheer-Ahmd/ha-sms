using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            {
                var cmd = new SqlCommand(@"
                    SELECT c.*, d.DepartmentName 
                    FROM Courses c 
                    INNER JOIN Departments d ON c.DepartmentID = d.DepartmentID", conn);
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
            {
                var cmd = new SqlCommand(@"
                    SELECT c.*, d.DepartmentName 
                    FROM Courses c 
                    INNER JOIN Departments d ON c.DepartmentID = d.DepartmentID
                    WHERE c.CourseID = @CourseID", conn);
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
            {
                var cmd = new SqlCommand(@"
                    SELECT c.*, d.DepartmentName 
                    FROM Courses c 
                    INNER JOIN Departments d ON c.DepartmentID = d.DepartmentID
                    WHERE c.CourseCode = @CourseCode", conn);
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
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO Courses (CourseCode, Title, Description, Credits, DepartmentID)
                    VALUES (@CourseCode, @Title, @Description, @Credits, @DepartmentID)", conn);
                
                cmd.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                cmd.Parameters.AddWithValue("@Title", course.Title);
                cmd.Parameters.AddWithValue("@Description", (object)course.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Credits", course.Credits);
                cmd.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCourse(Course course)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    UPDATE Courses 
                    SET CourseCode = @CourseCode, Title = @Title, Description = @Description, 
                        Credits = @Credits, DepartmentID = @DepartmentID
                    WHERE CourseID = @CourseID", conn);
                
                cmd.Parameters.AddWithValue("@CourseID", course.CourseID);
                cmd.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                cmd.Parameters.AddWithValue("@Title", course.Title);
                cmd.Parameters.AddWithValue("@Description", (object)course.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Credits", course.Credits);
                cmd.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCourse(int courseId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM Courses WHERE CourseID = @CourseID", conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Uses vw_CourseCatalog view
        public List<Course> GetCourseCatalog()
        {
            var courses = new List<Course>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM vw_CourseCatalog", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(new Course
                        {
                            CourseCode = reader["CourseCode"].ToString(),
                            Title = reader["CourseTitle"].ToString(),
                            Credits = (int)reader["Credits"],
                            Department = new Department { DepartmentName = reader["DepartmentName"].ToString() }
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
            {
                var cmd = new SqlCommand("SELECT * FROM Courses WHERE DepartmentID = @DepartmentID", conn);
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
            {
                var cmd = new SqlCommand(@"
                    SELECT cp.*, c.CourseCode, c.Title 
                    FROM CoursePrerequisites cp
                    INNER JOIN Courses c ON cp.PrerequisiteCourseID = c.CourseID
                    WHERE cp.CourseID = @CourseID", conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prerequisites.Add(new CoursePrerequisite
                        {
                            CourseID = (int)reader["CourseID"],
                            PrerequisiteCourseID = (int)reader["PrerequisiteCourseID"],
                            PrerequisiteCourse = new Course
                            {
                                CourseID = (int)reader["PrerequisiteCourseID"],
                                CourseCode = reader["CourseCode"].ToString(),
                                Title = reader["Title"].ToString()
                            }
                        });
                    }
                }
            }
            return prerequisites;
        }

        // Uses fn_CheckPrerequisitesMet function with CTEs
        public bool CheckPrerequisitesMet(int studentId, int courseId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT dbo.fn_CheckPrerequisitesMet(@StudentID, @CourseID)", conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null && Convert.ToBoolean(result);
            }
        }

        private Course MapCourse(IDataReader reader)
        {
            return new Course
            {
                CourseID = (int)reader["CourseID"],
                CourseCode = reader["CourseCode"].ToString(),
                Title = reader["Title"].ToString(),
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                Credits = (int)reader["Credits"],
                DepartmentID = (int)reader["DepartmentID"],
                Department = new Department
                {
                    DepartmentID = (int)reader["DepartmentID"],
                    DepartmentName = reader["DepartmentName"].ToString()
                }
            };
        }
    }
}
