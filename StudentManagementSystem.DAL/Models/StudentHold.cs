using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("StudentHolds")]
    public class StudentHold
    {
        [Key]
        public int HoldID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        [StringLength(50)]
        public string HoldType { get; set; }

        public string Reason { get; set; }

        [Required]
        public DateTime DateApplied { get; set; }

        public bool IsActive { get; set; }

        // Navigation properties
        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }

        public StudentHold()
        {
            IsActive = true;
            DateApplied = DateTime.Now;
        }
    }
}
