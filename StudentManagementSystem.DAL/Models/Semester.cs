using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("Semesters")]
    public class Semester
    {
        [Key]
        public int SemesterID { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [StringLength(50)]
        public string Season { get; set; }

        // Navigation properties
        public virtual ICollection<CourseOffering> CourseOfferings { get; set; }

        public Semester()
        {
            CourseOfferings = new HashSet<CourseOffering>();
        }

        [NotMapped]
        public string SemesterName => $"{Season} {Year}";
    }
}
