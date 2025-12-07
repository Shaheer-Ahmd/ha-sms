using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string EnrollmentStatus { get; set; }

        public DateTime? DateOfBirth { get; set; }

        // Navigation properties
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<StudentHold> StudentHolds { get; set; }

        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
            StudentHolds = new HashSet<StudentHold>();
            EnrollmentStatus = "Active";
        }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
