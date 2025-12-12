using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("CourseOfferings")]
    public class CourseOffering
    {
        [Key]
        public int OfferingID { get; set; }

        [Required]
        public int CourseID { get; set; }

        [Required]
        public int SemesterID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxCapacity { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CurrentEnrollment { get; set; }

        // Navigation properties
        [ForeignKey("CourseID")]
        public virtual Course? Course { get; set; }

        [ForeignKey("SemesterID")]
        public virtual Semester? Semester { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public CourseOffering()
        {
            Enrollments = new HashSet<Enrollment>();
            CurrentEnrollment = 0;
        }

        [NotMapped]
        public int SeatsRemaining => MaxCapacity - CurrentEnrollment;
    }
}
