using System.ComponentModel.DataAnnotations;
using HotelMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace HotelMS.ViewModel
{
    public class AccountVM
    {
        public AppRole? RoleIdentity { get; set; }
        public RoleVm? Role { get; set; }
        public IEnumerable<AppRole>? RolesTbList { get; set; } = new List<AppRole>();
        public Dictionary<string, List<string>> UserInRole { get; set; } = new Dictionary<string, List<string>>();
    }
}