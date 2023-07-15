using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;
using Web.UiState;

namespace Web.Pages.GroceryList
{
    [Authorize]
    public class GroceriesModel : PageModel
    {
        private readonly IGroceryFoodService _groceryFoodService;
        private readonly UserManager<User> _userManager;
        [BindProperty]
        public GroceryListUiState GroceryListUiState { get; set; } = new GroceryListUiState();

        public GroceriesModel(IGroceryFoodService groceryFoodService, UserManager<User> userManager)
        {
            _groceryFoodService = groceryFoodService;

            _userManager = userManager;
        }

        public async Task OnGet()
        {
           int household =  (int)(await _userManager.GetUserAsync(User)).HouseholdId;
           await _groceryFoodService.RetrieveFoodByHousehold(household);

        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _groceryFoodService.Delete(GroceryListUiState.DeleteId);
            return Redirect("/grocerylist/groceries");
           

        }

        public async Task<IActionResult> OnPostAsync()
        {
            using var streamReader = new StreamReader(GroceryListUiState.JsonFile.OpenReadStream());
            string jsonString = await streamReader.ReadToEndAsync();
            await _groceryFoodService.ImportGroceryList((int)(await _userManager.GetUserAsync(User)).HouseholdId, jsonString);
            return Redirect("/grocerylist/groceries");
        }

        
    }
}
