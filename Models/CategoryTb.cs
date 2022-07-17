using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMS.Models
{
    [Table("CategoryTb")]
    public class Category
    {
        public int? Id { get; set; }
        [Required]
        public string? CatName { get; set; }
        public string? RRemarks { get; set; }
    }
}