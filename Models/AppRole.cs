using Microsoft.AspNetCore.Identity;

namespace HotelMS.Models
{
    public class AppRole : IdentityRole<string>

    {
        public ICollection<AppUserRole>? UserRoles { get; set; }

    }
}