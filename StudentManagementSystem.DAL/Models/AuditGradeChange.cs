using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("Audit_GradeChanges")]
    public class AuditGradeChange
    {
        [Key, Column(Order = 0)]
        public int AuditID { get; set; }

        [Required]
        public int EnrollmentID { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public DateTime ChangeDate { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [StringLength(2)]
        public string OldGrade { get; set; }

        [StringLength(2)]
        public string NewGrade { get; set; }

        public int? ChangedByAdminID { get; set; }

        [StringLength(255)]
        public string ChangedByLogin { get; set; }

        public AuditGradeChange()
        {
            ChangeDate = DateTime.Now;
        }
    }
}
