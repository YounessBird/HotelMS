using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelMS.Models
{
    public class RoleVm
    {
        public string? Id { get; set; }

        [Display(Name = "Role Name")]
        public string RName { get; set; }

        public string? userId { get; set; }

    }

}