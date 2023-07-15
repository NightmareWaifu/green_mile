using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;

namespace Web.Pages.Recipes
{
    public class UpdateModel : PageModel
    {

        private readonly RecipeService _recipeService;
        private readonly FoodItemService _foodItemService;
        private IWebHostEnvironment _webHostEnvironment;


        public UpdateModel(RecipeService recipeService, IWebHostEnvironment webHostEnvironment, FoodItemService foodItemService)
        {
            _recipeService = recipeService;
            _webHostEnvironment = webHostEnvironment;
            _foodItemService = foodItemService;
        }

        [BindProperty]
        public Recipe CurrentRecipe { get; set; } = new();
        public IFormFile? image { get; set; }
        public List<String> testIngredientList { get; set; } = new();
        public List<FoodItem> IngredientList;


        public IActionResult OnGet(int id)
        {

            IngredientList = _foodItemService.GetAllForRecipe();
            //testIngredientList.Add("First ingredient");
            //testIngredientList.Add("Second ingredient");
            //testIngredientList.Add("Third ingredient");

            Recipe? recipe = _recipeService.GetRecipeById(id);
            if(recipe == null)
            {
                TempData["error"] = "Recipe not found!";
                return Redirect("/Recipes/BackendView");
            }
            CurrentRecipe = recipe;
            //get ingredients here
            return Page();


        }
        public IActionResult OnPost()
        {
            if (true)   
            { 
                if (image != null)
                {

                    var imagesFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", imagesFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    image.CopyTo(fileStream);
                    CurrentRecipe.imageFilePath = String.Format("/" + imagesFolder + "/" + imageFile);

                }
                _recipeService.UpdateRecipe(CurrentRecipe);
                TempData["success"] = CurrentRecipe.recipeName + " successfully updated!";
                return Redirect("/Recipes/Index");
            }
            //return Page();
        }
    }
}
