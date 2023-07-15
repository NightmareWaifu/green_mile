namespace Web.Models
{
    public class SearchFoodItem
    {
        public int Id { get; set; }
       
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Weight { get; set; }
        public string ImageFilePath { get; set; } = string.Empty;
        public string Category { get; set; }
       

    }
}
