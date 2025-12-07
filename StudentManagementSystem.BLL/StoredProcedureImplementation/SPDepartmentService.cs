using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.StoredProcedureImplementation
{
    public class SPDepartmentService : IDepartmentService
    {
        private readonly string _connectionString;

        public SPDepartmentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Department> GetAllDepartments()
        {
            var departments = new List<Department>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Departments", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            DepartmentID = (int)reader["DepartmentID"],
                            DepartmentName = reader["DepartmentName"].ToString(),
                            ParentDepartmentID = reader["ParentDepartmentID"] == DBNull.Value ? (int?)null : (int)reader["ParentDepartmentID"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return departments;
        }

        public Department GetDepartmentById(int departmentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Departments WHERE DepartmentID = @DepartmentID", conn);
                cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Department
                        {
                            DepartmentID = (int)reader["DepartmentID"],
                            DepartmentName = reader["DepartmentName"].ToString(),
                            ParentDepartmentID = reader["ParentDepartmentID"] == DBNull.Value ? (int?)null : (int)reader["ParentDepartmentID"],
                            IsActive = (bool)reader["IsActive"]
                        };
                    }
                }
            }
            return null;
        }

        public void AddDepartment(Department department)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO Departments (DepartmentName, ParentDepartmentID, IsActive)
                    VALUES (@DepartmentName, @ParentDepartmentID, @IsActive)", conn);
                
                cmd.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                cmd.Parameters.AddWithValue("@ParentDepartmentID", (object)department.ParentDepartmentID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", department.IsActive);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateDepartment(Department department)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"
                    UPDATE Departments 
                    SET DepartmentName = @DepartmentName, ParentDepartmentID = @ParentDepartmentID, IsActive = @IsActive
                    WHERE DepartmentID = @DepartmentID", conn);
                
                cmd.Parameters.AddWithValue("@DepartmentID", department.DepartmentID);
                cmd.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                cmd.Parameters.AddWithValue("@ParentDepartmentID", (object)department.ParentDepartmentID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", department.IsActive);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM Departments WHERE DepartmentID = @DepartmentID", conn);
                cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Uses sp_GetDepartmentHierarchy with recursive CTE
        public DataTable GetDepartmentHierarchy()
        {
            var dataTable = new DataTable();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("sp_GetDepartmentHierarchy", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        public List<Department> GetActiveDepartments()
        {
            var departments = new List<Department>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Departments WHERE IsActive = 1", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            DepartmentID = (int)reader["DepartmentID"],
                            DepartmentName = reader["DepartmentName"].ToString(),
                            ParentDepartmentID = reader["ParentDepartmentID"] == DBNull.Value ? (int?)null : (int)reader["ParentDepartmentID"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return departments;
        }
    }
}
