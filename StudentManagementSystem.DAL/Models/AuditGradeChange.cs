using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("Audit_GradeChanges")]
    public class AuditGradeChange
    {
        [Key]
        public int AuditID { get; set; }

        public int EnrollmentID { get; set; }

        public DateTime EnrollmentDate { get; set; }

        [StringLength(2)]
        public string? OldGrade { get; set; }

        [StringLength(2)]
        public string? NewGrade { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}
