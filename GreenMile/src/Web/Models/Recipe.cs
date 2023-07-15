using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Web.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Recipe name is required!"), MaxLength(50, ErrorMessage = "Recipe name too long!")]
    
        public string recipeName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter the recipe description!")]
        public string description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select an ingredient!")]
        public string ingredients { get; set; } = string.Empty; //convert to string before sending to the database
        //it is a string of ids separated by ","
        [Required]
        public string ingredientAmount { get; set; } = string.Empty; //same as before, match index 1 to 1

        [Required(ErrorMessage = "Please include the recipe duration!")]
        public int duration { get; set; } = 0;

        [Required(ErrorMessage = "Please select a difficulty!")]
        public string difficulty { get; set; } = string.Empty ;

        public string? imageFilePath { get; set; }

        //image?

    }
}
