using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMS.Models
{
    public class User
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "Please fill in the Name field")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string? UName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? UPhone { get; set; }
        public string? UGender { get; set; }
        [Required(ErrorMessage = "Please fill in the email address field")]
        [EmailAddress(ErrorMessage = "Please Enter a Valid Email Address")]
        [Display(Name = "Email")]
        public string? UEmail { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? UPassword { get; set; }
    }
}
