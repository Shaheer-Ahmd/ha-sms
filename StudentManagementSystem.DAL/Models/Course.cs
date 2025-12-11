using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        [StringLength(255)]
        public string CourseCode { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, 6)]
        public int Credits { get; set; }

        [Required]
        public int DepartmentID { get; set; }

        // Navigation properties
        [ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; }

        public virtual ICollection<CourseOffering> CourseOfferings { get; set; }

        public Course()
        {
            CourseOfferings = new HashSet<CourseOffering>();
        }
    }
}
