namespace Web.Models
{
    public class JsonGroceryFoodItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Quantity { get; set; }
        public string? ExtraNote { get; set; }
        public bool InBasket { get; set; } = false;



        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? HouseholdId { get; set; }
        public double? CarbonFootprint { get; set; }

        public string? ImageFilePath { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
    }
}
