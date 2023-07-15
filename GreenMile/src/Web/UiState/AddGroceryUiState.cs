using Microsoft.AspNetCore.Mvc;

using Web.Models;

namespace Web.UiState
{
    public class AddGroceryUiState
    {
        
        public string? Name { get; set; }
     
        public IFormFile? Image { get; set; }

        public int Quantity { get; set; }

        public int Category { get; set; }

        public string? Description { get; set; } = "";

       
        
      
    }
}
