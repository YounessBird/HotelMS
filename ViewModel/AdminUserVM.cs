using System.ComponentModel.DataAnnotations;
using HotelMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelMS.ViewModel
{
    public class AdminUserVM
    {
        public UserId? Id { get; set; }
        public User User { get; set; } // this has to extend from IdentityUser
        public List<UserDetailsDto>? UserList { get; set; } // Note this may have to change to IdentityUser at the moment it's converted from identityuser to User in the controller 
        public Category Category { get; set; }
        // this might be obsolete
        public List<Category> CatList { get; set; }
        public List<SelectListItem> CatListItem { get; set; }
        public Room Room { get; set; }
        // this might be obsolete
        public List<Room> RoomList { get; set; }
        public List<AppRole> RolesList { get; set; }
        public AppRole? Role { get; set; }
        public List<UserRoles>? UserRoles { get; set; } = new List<UserRoles>();
        public Dictionary<string, List<AppRole>> RolesInUsers { get; set; } = new Dictionary<string, List<AppRole>>();
    }
}