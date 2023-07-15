using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class FoodItem
    {
        [Key]
        public int Id { get; set; }
        public Household? Household { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public double CarbonFootprint { get; set; }
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public double Cost { get; set; }
        public string ImageFilePath { get; set; } = string.Empty;
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public string Category { get; set; }
        public bool IsCustom { get; set; } 

        public bool Status { get; set; } 



    }
}
