using System;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.DAL
{
    public class StudentManagementContext : DbContext
    {
        private string? _connectionString;

        public StudentManagementContext() { }

        public StudentManagementContext(DbContextOptions<StudentManagementContext> options)
            : base(options)
        {
        }

        public StudentManagementContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        // DbSets for all entities
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CoursePrerequisite> CoursePrerequisites { get; set; } = null!;
        public virtual DbSet<CourseOffering> CourseOfferings { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<StudentHold> StudentHolds { get; set; } = null!;
        public virtual DbSet<AuditGradeChange> AuditGradeChanges { get; set; } = null!;

        // Views
        public virtual DbSet<StudentTranscript> StudentTranscripts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _connectionString ?? "Server=localhost,1433;Database=StudentManagementDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department - Self-referencing
            modelBuilder.Entity<Department>()
                .HasOne(d => d.ParentDepartment)
                .WithMany(d => d.ChildDepartments)
                .HasForeignKey(d => d.ParentDepartmentID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Course - Department
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // CoursePrerequisite
            modelBuilder.Entity<CoursePrerequisite>()
                .HasKey(cp => new { cp.CourseID, cp.PrerequisiteCourseID });

            modelBuilder.Entity<CoursePrerequisite>()
                .HasOne(cp => cp.PrerequisiteCourse)
                .WithMany()
                .HasForeignKey(cp => cp.PrerequisiteCourseID)
                .OnDelete(DeleteBehavior.Restrict);

            // CourseOffering - Course and Semester
            modelBuilder.Entity<CourseOffering>()
                .HasOne(co => co.Course)
                .WithMany(c => c.CourseOfferings)
                .HasForeignKey(co => co.CourseID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CourseOffering>()
                .HasOne(co => co.Semester)
                .WithMany(s => s.CourseOfferings)
                .HasForeignKey(co => co.SemesterID)
                .OnDelete(DeleteBehavior.Restrict);

            // Enrollment - Composite Key
   modelBuilder.Entity<Enrollment>(entity =>
    {
        entity.HasKey(e => new { e.EnrollmentID, e.EnrollmentDate });

        entity.Property(e => e.EnrollmentID)
              .ValueGeneratedOnAdd();

        entity.Property(e => e.EnrollmentDate)
              .HasColumnType("date");

        // Relationships
        entity.HasOne(e => e.Student)
              .WithMany(s => s.Enrollments)
              .HasForeignKey(e => e.StudentID)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.CourseOffering)
              .WithMany(co => co.Enrollments)
              .HasForeignKey(e => e.OfferingID)
              .OnDelete(DeleteBehavior.Restrict);

        entity.ToTable(tb => tb.UseSqlOutputClause(false));
    });

            // StudentHold
            modelBuilder.Entity<StudentHold>()
                .HasOne(sh => sh.Student)
                .WithMany(s => s.StudentHolds)
                .HasForeignKey(sh => sh.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            // AuditGradeChange - Composite Key
            // modelBuilder.Entity<AuditGradeChange>()
            //     .HasKey(ag => new { ag.AuditID, ag.ChangeDate });
            modelBuilder.Entity<AuditGradeChange>()
            .ToTable("Audit_GradeChanges")
            .HasKey(a => a.AuditID);
            // StudentTranscript is a view - configure as view
            modelBuilder.Entity<StudentTranscript>()
                .ToView("StudentTranscript")
                .HasKey(st => new { st.StudentID, st.CourseCode, st.Year, st.Season });
        }
    }
}
