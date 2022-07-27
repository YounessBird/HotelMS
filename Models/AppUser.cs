
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HotelMS.Models
{
    [Table("UserTb")]
    public class AppUser : IdentityUser<string>
    {
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }

        public string? GenericPassword { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
