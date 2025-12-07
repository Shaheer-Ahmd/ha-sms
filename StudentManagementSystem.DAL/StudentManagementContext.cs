using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.DAL
{
    public class StudentManagementContext : DbContext
    {
        public StudentManagementContext() : base("name=StudentManagementDB")
        {
            // Disable lazy loading for better control
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public StudentManagementContext(string connectionString) : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        // DbSets for all entities
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }
        public virtual DbSet<CourseOffering> CourseOfferings { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<StudentHold> StudentHolds { get; set; }
        public virtual DbSet<AuditGradeChange> AuditGradeChanges { get; set; }

        // Views
        public virtual DbSet<StudentTranscript> StudentTranscripts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Remove pluralizing convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Department - Self-referencing
            modelBuilder.Entity<Department>()
                .HasOptional(d => d.ParentDepartment)
                .WithMany(d => d.ChildDepartments)
                .HasForeignKey(d => d.ParentDepartmentID);

            // Course - Department
            modelBuilder.Entity<Course>()
                .HasRequired(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentID)
                .WillCascadeOnDelete(false);

            // CoursePrerequisite - Composite Key
            modelBuilder.Entity<CoursePrerequisite>()
                .HasKey(cp => new { cp.CourseID, cp.PrerequisiteCourseID });

            modelBuilder.Entity<CoursePrerequisite>()
                .HasRequired(cp => cp.Course)
                .WithMany(c => c.Prerequisites)
                .HasForeignKey(cp => cp.CourseID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CoursePrerequisite>()
                .HasRequired(cp => cp.PrerequisiteCourse)
                .WithMany()
                .HasForeignKey(cp => cp.PrerequisiteCourseID)
                .WillCascadeOnDelete(false);

            // CourseOffering - Course and Semester
            modelBuilder.Entity<CourseOffering>()
                .HasRequired(co => co.Course)
                .WithMany(c => c.CourseOfferings)
                .HasForeignKey(co => co.CourseID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CourseOffering>()
                .HasRequired(co => co.Semester)
                .WithMany(s => s.CourseOfferings)
                .HasForeignKey(co => co.SemesterID)
                .WillCascadeOnDelete(false);

            // Enrollment - Composite Key for partitioned table
            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.EnrollmentID, e.EnrollmentDate });

            modelBuilder.Entity<Enrollment>()
                .HasRequired(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Enrollment>()
                .HasRequired(e => e.CourseOffering)
                .WithMany(co => co.Enrollments)
                .HasForeignKey(e => e.OfferingID)
                .WillCascadeOnDelete(false);

            // StudentHold
            modelBuilder.Entity<StudentHold>()
                .HasRequired(sh => sh.Student)
                .WithMany(s => s.StudentHolds)
                .HasForeignKey(sh => sh.StudentID)
                .WillCascadeOnDelete(false);

            // AuditGradeChange - Composite Key for partitioned table
            modelBuilder.Entity<AuditGradeChange>()
                .HasKey(ag => new { ag.AuditID, ag.ChangeDate });

            // StudentTranscript is a view - no key needed, read-only
            modelBuilder.Entity<StudentTranscript>()
                .HasKey(st => new { st.StudentID, st.CourseCode, st.Year, st.Season });
        }
    }
}
