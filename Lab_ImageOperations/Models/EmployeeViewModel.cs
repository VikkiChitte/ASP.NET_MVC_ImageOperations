using System.ComponentModel.DataAnnotations;

namespace Lab_ImageOperations.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="Please Enter Your Name!")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Please Enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage ="Please Select Image")]
        [Display(Name="Picture")]
        public IFormFile Image { get; set; }
        public string? ImagePath { get; set; }
    }
}
