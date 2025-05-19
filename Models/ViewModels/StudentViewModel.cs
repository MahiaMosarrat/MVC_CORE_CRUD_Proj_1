using System.ComponentModel.DataAnnotations;

namespace CoreFinal.Models.ViewModels
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public bool IsEnrolled { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AdmissionDate { get; set; }
        public decimal RegistrationFee { get; set; }
        public string? ImageUrl { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
        public virtual IList<Course>? Courses { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
