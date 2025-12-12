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
    using (var context = new StudentManagementContext(_connectionString))
    {
        var department = context.Departments.Find(departmentId);
        if (department != null)
        {
            context.Departments.Remove(department);
            context.SaveChanges();
        }
    }
}

public DataTable GetDepartmentHierarchy()
{
    using (var context = new StudentManagementContext(_connectionString))
    {
        // 1) Load all departments
        var departments = context.Departments.ToList();

        // 2) Prepare result DataTable (same schema as sp_GetDepartmentHierarchy)
        var table = new DataTable();
        table.Columns.Add("DepartmentID", typeof(int));
        table.Columns.Add("IndentedName", typeof(string));
        table.Columns.Add("Level", typeof(int));
        table.Columns.Add("HierarchyPath", typeof(string));

        // 3) Roots: ParentDepartmentID == null
        var roots = departments
            .Where(d => d.ParentDepartmentID == null)
            .OrderBy(d => d.DepartmentName)
            .ToList();

        // 4) Children lookup: key is NON-NULL parent ID
        var childrenLookup = departments
            .Where(d => d.ParentDepartmentID != null)
            .GroupBy(d => d.ParentDepartmentID!.Value)
            .ToDictionary(
                g => g.Key,                            // parent DepartmentID
                g => g.OrderBy(x => x.DepartmentName)  // children ordered by name
                      .ToList()
            );

        // 5) Recursive traversal
        void Traverse(Department dept, int level, string parentPath)
        {
            var hierarchyPath = string.IsNullOrEmpty(parentPath)
                ? dept.DepartmentName
                : parentPath + " > " + dept.DepartmentName;

            var indentedName = new string(' ', level * 4) + dept.DepartmentName;

            table.Rows.Add(
                dept.DepartmentID,
                indentedName,
                level,
                hierarchyPath
            );

            if (childrenLookup.TryGetValue(dept.DepartmentID, out var children))
            {
                foreach (var child in children)
                {
                    Traverse(child, level + 1, hierarchyPath);
                }
            }
        }

        // 6) Kick off from roots
        foreach (var root in roots)
        {
            Traverse(root, level: 0, parentPath: string.Empty);
        }

        return table;
    }
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
