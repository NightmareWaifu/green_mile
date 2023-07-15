using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Message
{
    [Key]
    public int Key { get; set; }
    public User User { get; set; }
    public Household Household { get; set; }
    public string Text { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}