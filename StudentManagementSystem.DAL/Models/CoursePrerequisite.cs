using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.DAL.Models
{
    [Table("CoursePrerequisites")]
    public class CoursePrerequisite
    {
        [Key, Column(Order = 0)]
        public int CourseID { get; set; }

        [Key, Column(Order = 1)]
        public int PrerequisiteCourseID { get; set; }

        // Navigation properties
        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("PrerequisiteCourseID")]
        public virtual Course PrerequisiteCourse { get; set; }
    }
}
