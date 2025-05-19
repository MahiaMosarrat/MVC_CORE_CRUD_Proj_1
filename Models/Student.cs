using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CoreFinal.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public bool IsEnrolled { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:yyyy-MM-dd}")]
        public DateTime AdmissionDate { get; set; }
        public decimal RegistrationFee { get; set; }
        public string? ImageUrl { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    }
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = null!;
        public int Duration { get; set; }
        public int StudentId { get; set; }
        public virtual Student? Student { get; set; }
    }
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>op):base(op)
        {
            
        }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId=1,CourseName="C#"},
                new Course { CourseId = 2, CourseName = "NT" }
                );
        }
    }
}
