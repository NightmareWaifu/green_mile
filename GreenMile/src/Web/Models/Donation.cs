using System.ComponentModel.DataAnnotations;

namespace Web.Models;


public class Donation
{

    [Key]
    public int Id { get; set; }
    public DonationStatus Status { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string Location { get; set; } = string.Empty;

    public User User { get; set; } = new User();

    public CustomFood CustomFood { get; set; }  = new CustomFood();

}

public enum DonationStatus
{
    ACTIVE,
    RESERVED,
    COMPLETED,
    CANCELLED
}