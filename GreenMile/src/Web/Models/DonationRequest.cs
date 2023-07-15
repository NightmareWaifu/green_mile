using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class DonationRequest
    {
        [Key]
        public int Id { get; set; }
        public Donation? Donation { get; set; }

        public User? Donor { get; set; }

        public User? Recipient { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

    }
}

public enum RequestStatus
{
    PENDING,
    CANCELLED,
    ACCEPTED,
    DONE,
}
