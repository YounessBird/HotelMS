using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMS.Models
{
    [Table("RoomTb")]
    public class Room
    {
        public int? Id { get; set; }
        [Required]
        public string? RName { get; set; }
        public int? RCategoryId { get; set; }
        [ForeignKey("RCategoryId")]
        public Category? RCategory { get; set; }
        [Required]
        public string? RLocation { get; set; }
        [Required]
        public int? RCost { get; set; }
        [Required]
        public string? RRemarks { get; set; }
        [Required]
        public string? Status { get; set; }

    }
}
