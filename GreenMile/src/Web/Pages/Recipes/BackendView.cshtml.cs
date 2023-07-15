using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;

namespace Web.Pages.Recipes
{
    public class BackendViewModel : PageModel
    {

        private readonly RecipeService _recipeService;

       public BackendViewModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [BindProperty]
        public List<Recipe> RecipeList { get; set; } = new();
        public void OnGet()
        {
            RecipeList = _recipeService.GetAll();
        }

        public PageResult OnGetDelete(int id)
        {
            Recipe? recipe = _recipeService.GetRecipeById(id);
            TempData["success"] = recipe.recipeName + " successfully Deleted!";
            _recipeService.DeleteRecipe(recipe);
            RecipeList = _recipeService.GetAll();
            return Page();
        }


    }
}
