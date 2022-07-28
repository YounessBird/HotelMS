using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace HotelMS.ViewModels
{
    [Table("BookingTb")]
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        //[Remote(action: "IsEmailTaken", controller: "Account")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

    }
}