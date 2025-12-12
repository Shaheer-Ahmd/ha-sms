using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
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
            using (var cmd = new SqlCommand("sp_Department_GetAll", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(MapDepartment(reader));
                    }
                }
            }

            return departments;
        }

        public Department GetDepartmentById(int departmentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Department_GetById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentID", departmentId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapDepartment(reader);
                    }
                }
            }

            return null;
        }

        public void AddDepartment(Department department)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Department_Add", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DepartmentName",     department.DepartmentName);
                cmd.Parameters.AddWithValue("@ParentDepartmentID",
                    (object?)department.ParentDepartmentID ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive",           department.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateDepartment(Department department)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Department_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DepartmentID",       department.DepartmentID);
                cmd.Parameters.AddWithValue("@DepartmentName",     department.DepartmentName);
                cmd.Parameters.AddWithValue("@ParentDepartmentID",
                    (object?)department.ParentDepartmentID ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive",           department.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Department_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentID", departmentId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetDepartmentHierarchy()
        {
            var table = new DataTable();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetDepartmentHierarchy", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        public List<Department> GetActiveDepartments()
        {
            var departments = new List<Department>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_Department_GetActive", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(MapDepartment(reader));
                    }
                }
            }

            return departments;
        }

        private Department MapDepartment(IDataRecord reader)
        {
            return new Department
            {
                DepartmentID       = (int)reader["DepartmentID"],
                DepartmentName     = reader["DepartmentName"].ToString(),
                ParentDepartmentID = reader["ParentDepartmentID"] == System.DBNull.Value
                    ? (int?)null
                    : (int)reader["ParentDepartmentID"],
                IsActive           = (bool)reader["IsActive"]
            };
        }
    }
}
