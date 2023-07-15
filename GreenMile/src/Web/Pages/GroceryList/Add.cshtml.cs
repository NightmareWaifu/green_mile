using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;
using Web.UiState;

namespace Web.Pages.GroceryList
{
    public class AddModel : PageModel
    {
        [BindProperty,Required]
        public AddGroceryUiState GroceryUiState { get; set; }

        public List<Category> Categories { get; set; }
        private readonly IImageService _imageService;
        private readonly IHouseholdService _householdService;
        private readonly IGroceryFoodService _foodService;
        private readonly UserManager<User> _userManager;
        private readonly CategoryService _categoryService;

        public AddModel(IGroceryFoodService foodService, IImageService imageService, IHouseholdService householdService, UserManager<User> userManager, CategoryService categoryService)
        {
            _foodService = foodService;
            _imageService = imageService;
            _householdService = householdService;
            _userManager = userManager;
            _categoryService = categoryService;
        }

 

        public async Task OnGet()
        {
            Categories = await _categoryService.RetrieveCategories();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.GetUserAsync(User);
            if(!(user is null))
            {
                var item = new GroceryFoodItem()
                {
                    Id = Guid.NewGuid().ToString(),
                Household = user.Household,
                HouseholdId = user.HouseholdId?? 0,
                    Name = GroceryUiState.Name,
                    Quantity = GroceryUiState.Quantity,
                    ImageFilePath = await _imageService.StoreImage(GroceryUiState.Image),
                    CategoryId = GroceryUiState.Category,
                    Description = GroceryUiState.Description
                    
                };
                await _foodService.Add(item);
       

      
                TempData["success"] = "Added!";
                return Redirect("/grocerylist/groceries");


            }
            return Page();
        }
    }
}
