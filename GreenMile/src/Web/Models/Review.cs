using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Web.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string userID { get; set; } = String.Empty;
        [Required]
        public int recipeID { get; set; } = 0;
        public string? description { get; set; } = String.Empty;
        public int rating { get; set; } 
    }
}
