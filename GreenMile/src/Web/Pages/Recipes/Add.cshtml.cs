using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Models;
using Web.Services;

namespace Web.Pages.Recipes
{
    public class AddModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private readonly FoodItemService _foodItemService;
        private IWebHostEnvironment _webHostEnvironment;


        public AddModel(RecipeService recipeService, IWebHostEnvironment webHostEnvironment, FoodItemService foodItemService)
        {
            _recipeService = recipeService;
            _webHostEnvironment = webHostEnvironment;
            _foodItemService = foodItemService;
        }

        [BindProperty]
        public Recipe CurrentRecipe { get; set; } = new();
        public IFormFile? image { get; set; }
        //public List<String> testIngredientList { get; set; } = new();

        public List<FoodItem> IngredientList;

        public List<string> ingredientIds;
        public List<string> ingredientAmounts;

        public void OnGet()
        {
            //get the ingredients list
             IngredientList =  _foodItemService.GetAllForRecipe();

            //test data
            //testIngredientList.Add("First ingredient");
            //testIngredientList.Add("Second ingredient");
            //testIngredientList.Add("Third ingredient");
        }
        public IActionResult OnPost()
        {
            IngredientList = _foodItemService.GetAllForRecipe();
            if (ModelState.IsValid)
            {  
                if (image != null)
                {

                    var imagesFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath,"wwwroot",imagesFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    image.CopyTo(fileStream);
                    CurrentRecipe.imageFilePath = String.Format("/" + imagesFolder + "/" + imageFile);

                }
                Recipe? recipe = _recipeService.GetRecipeById(CurrentRecipe.Id);
                if(recipe != null)
                {
                    TempData["error"] = CurrentRecipe.recipeName + " already exists!";
                    return Page();
                }
                _recipeService.AddRecipe(CurrentRecipe);
                TempData["success"] = CurrentRecipe.recipeName + " successfully added!";
                return Redirect("/Recipes/Index");
            }
            TempData["error"] = "Please fill in all the fields.";
            return Page();
        }
    }
}
