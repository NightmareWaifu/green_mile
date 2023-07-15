using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;

namespace Web.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private readonly FoodItemService _foodItemService;
        private ReviewService _reviewService;
        private readonly UserManager<User> _userManager;
        public Recipe currentRecipe { get; set; } = new();

        public DetailsModel(RecipeService recipeService, FoodItemService foodItemService, ReviewService reviewService, UserManager<User> userManager)
        {
            _recipeService = recipeService;
            _foodItemService = foodItemService;
            _reviewService = reviewService;
            _userManager = userManager;
        }
        [BindProperty]
        public List<string> ingredients { get; set; } = new();
        [BindProperty]
        public Review review { get; set; } = new();
        [BindProperty]
        public List<Review> allReviews { get; set; } = new();
        //public List<FoodItem> allFoodItems { get; set; } = new();
        public int averageRating;
        public void OnGet(int id)
        {
            User loggedInUser = _userManager.GetUserAsync(User).Result;
            currentRecipe = _recipeService.GetRecipeById(id);
            if (currentRecipe == null)
            {
                id = (int)HttpContext.Session.GetInt32("recipeID");
                currentRecipe = _recipeService.GetRecipeById(id);
            }
            else
            {
                HttpContext.Session.SetInt32("recipeID", id);
            }
            
            List<string> result = currentRecipe.ingredients?.Split(',').ToList();
            foreach(var ingredientID in result)
            {
                //int index = Int32.Parse(ingredientID);
                FoodItem foodItem = _foodItemService.GetFoodItemById(Int32.Parse(ingredientID)).Result;
                ingredients.Add(foodItem.Name);
            }
            allReviews = _reviewService.getAllReviews(id);
            averageRating = 0;
            foreach(var ratings in allReviews)
            {
                averageRating += ratings.rating;
            }
            if (allReviews.Count == 0)
            {
                averageRating = 0;
            }
            else
            {
                averageRating = (averageRating / allReviews.Count) * 20;
            } //multiply by 20 since we are displaying it as a width percentage (style)

        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

            
                User loggedInUser = _userManager.GetUserAsync(User).Result;
                if (loggedInUser == null)
                {
                    TempData["error"] = "Log in to add a review!";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("RATING: " + review.rating.ToString());
                   
                        var NewReview = new Review()
                        {
                            userID = loggedInUser.Id.ToString(),
                            recipeID = (int)HttpContext.Session.GetInt32("recipeID"),
                            description = (review.description is null) ? "User has not provided a description" : review.description.ToString(),
                            rating = review.rating
                        };
                        _reviewService.addReview(NewReview);



                    TempData["success"] = "Review successfully added!";
                    return RedirectToPage("/Recipes/Details");
                }
            }
            TempData["error"] = "Please log in to add a review!";
            return RedirectToPage("/Recipes/Details");
            //return Page();

        }
    }
}
