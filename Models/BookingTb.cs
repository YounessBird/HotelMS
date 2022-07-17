using System.ComponentModel.DataAnnotations.Schema;

namespace HotelMS.Models
{
    [Table("BookingTb")]
    public class Booking
    {
        public int Id { get; set; }
        public DateTime? BDate { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room? BRoom { get; set; }
        public int AgentId { get; set; }
        [ForeignKey("AgentId")]
        public User Agent { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public int? Amount { get; set; }
    }
}