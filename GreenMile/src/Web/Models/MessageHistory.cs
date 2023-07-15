using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class MessageHistory
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public DonationRequest DonationRequest { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
