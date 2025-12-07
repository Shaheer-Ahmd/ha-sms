using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(255)]
        public string DepartmentName { get; set; }

        public int? ParentDepartmentID { get; set; }

        public bool IsActive { get; set; }

        // Navigation properties
        [ForeignKey("ParentDepartmentID")]
        public virtual Department ParentDepartment { get; set; }

        public virtual ICollection<Department> ChildDepartments { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public Department()
        {
            ChildDepartments = new HashSet<Department>();
            Courses = new HashSet<Course>();
            IsActive = true;
        }
    }
}
