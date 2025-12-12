using System.Collections.Generic;
using System.Data;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.BLL.Interfaces
{
    public interface IDepartmentService
    {
        List<Department> GetAllDepartments();
        Department GetDepartmentById(int departmentId);
        void AddDepartment(Department department);
        void UpdateDepartment(Department department);
        void DeleteDepartment(int departmentId);

        DataTable GetDepartmentHierarchy(); // Uses sp_GetDepartmentHierarchy with CTE
        List<Department> GetActiveDepartments();
    }
}
