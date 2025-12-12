using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("Enrollments")]
    public class Enrollment
    {
        [Key, Column(Order = 0)]
        public int EnrollmentID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public int OfferingID { get; set; }

        [StringLength(2)]
        public string? Grade { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public DateTime EnrollmentDate { get; set; }

        // Navigation properties
        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }

        [ForeignKey("OfferingID")]
        public virtual CourseOffering CourseOffering { get; set; }
    }
}
