using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.LinqImplementation
{
    public class LinqDepartmentService : IDepartmentService
    {
        private readonly string _connectionString;

        public LinqDepartmentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Department> GetAllDepartments()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Departments.ToList();
            }
        }

        public Department GetDepartmentById(int departmentId)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Departments.FirstOrDefault(d => d.DepartmentID == departmentId);
            }
        }

        public void AddDepartment(Department department)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Departments.Add(department);
                context.SaveChanges();
            }
        }

        public void UpdateDepartment(Department department)
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                context.Entry(department).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Departments WHERE DepartmentID = @DepartmentID";
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Uses sp_GetDepartmentHierarchy with recursive CTE
        public DataTable GetDepartmentHierarchy()
        {
            var dataTable = new DataTable();
            using (var context = new StudentManagementContext(_connectionString))
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_GetDepartmentHierarchy", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            return dataTable;
        }

        public List<Department> GetActiveDepartments()
        {
            using (var context = new StudentManagementContext(_connectionString))
            {
                return context.Departments
                    .Where(d => d.IsActive)
                    .OrderBy(d => d.DepartmentName)
                    .ToList();
            }
        }
    }
}
