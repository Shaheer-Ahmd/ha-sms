using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    // This represents the vw_StudentTranscript view
    [Table("vw_StudentTranscript")]
    public class StudentTranscript
    {
        [Key, Column(Order = 0)]
        public int StudentID { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(255)]
        public string CourseCode { get; set; }

        [StringLength(255)]
        public string CourseTitle { get; set; }

        public int Credits { get; set; }

        [Key, Column(Order = 2)]
        public int Year { get; set; }

        [Key, Column(Order = 3)]
        [StringLength(50)]
        public string Season { get; set; }

        [StringLength(2)]
        public string Grade { get; set; }

        [NotMapped]
        public string StudentName => $"{FirstName} {LastName}";

        [NotMapped]
        public string SemesterInfo => $"{Season} {Year}";
    }
}
