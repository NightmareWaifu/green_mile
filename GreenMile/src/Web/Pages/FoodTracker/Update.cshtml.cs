    
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;


namespace Web.Pages.FoodTracker;

public class UpdateModel : PageModel

{
    private readonly FoodItemService _foodItemService;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _http;
    private readonly IHouseholdService _householdService;
    private readonly FoodCategoryService _foodcategoryService;
    private IWebHostEnvironment _environment;

    public UpdateModel(IHttpContextAccessor http, IHouseholdService householdService, FoodItemService fooditemService, IWebHostEnvironment environment, FoodCategoryService foodCategoryService)
    {
        _foodItemService = fooditemService;
        _environment = environment;
        _foodcategoryService = foodCategoryService;
        _http = http;
        _householdService = householdService;
    }
   
    [BindProperty]
    public FoodItem CurrentFoodItem { get; set; }

    public static List<Category> Categories { get; set; } = new();

    [BindProperty, Required]
    public string Category { get; set; }

    [BindProperty, Required]
    public string Name { get; set; }

    [BindProperty, Required, MinLength(0), MaxLength(100)]
    public string Description { get; set; }

    [BindProperty, Required, Range(1, 100, ErrorMessage = " Choose between 1 - 100")]
    public int Quantity { get; set; }

    [BindProperty, Required]
    public DateTime ExpiryDate { get; set; }

    [BindProperty, Required]
    public IFormFile? ImageFilePath { get; set; }

    [BindProperty, Required]
    public double Weight { get; set; }

    [BindProperty, Required]
    public double CarbonFoodprint { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        CurrentFoodItem = await _foodItemService.GetFoodItemById(id);
        Categories.Add(new Category { Id = 1, Name = "Fruit", Description = "healthy" });
        Categories.Add(new Category { Id = 2, Name = "Meat", Description = "yummy" });
        Categories.Add(new Category { Id = 3, Name = "Vegetable", Description = "healthy" });
        Categories.Add(new Category { Id = 4, Name = "Dairy", Description = "tasty" });

        if (CurrentFoodItem is null)
        {
            return NotFound();
        }

        Name = CurrentFoodItem.Name;
        Description = CurrentFoodItem.Description;
        Quantity = CurrentFoodItem.Quantity;
        ExpiryDate = CurrentFoodItem.ExpiryDate;
        Category = CurrentFoodItem.Category;


        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (ImageFilePath != null)
        {
            if (ImageFilePath.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                return Page();
            }
           


            var uploadsFolder = "uploads";
            if (CurrentFoodItem.ImageFilePath != null)
            {
                var oldImageFile = Path.GetFileName(CurrentFoodItem.ImageFilePath);
                var oldImagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, oldImageFile);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                // Console.WriteLine("Success 1");
            }
            // Console.WriteLine("Success 2");

            var imageFile = Guid.NewGuid() + Path.GetExtension(ImageFilePath.FileName);
            var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
            using var fileStream = new FileStream(imagePath, FileMode.Create);
            await ImageFilePath.CopyToAsync(fileStream);
            CurrentFoodItem.ImageFilePath = string.Format("/{0}/{1}", uploadsFolder, imageFile);
            // Console.WriteLine("Success 3");
        }

        var category = _foodcategoryService.GetCategoryByName(Category);

        CurrentFoodItem.Name = Name;
        CurrentFoodItem.Description = Description;
        CurrentFoodItem.Quantity = Quantity;
        CurrentFoodItem.ExpiryDate = ExpiryDate;
        CurrentFoodItem.Category = Category;
        CurrentFoodItem.CarbonFootprint = CarbonFoodprint;
        CurrentFoodItem.Weight = Weight;
        CurrentFoodItem.Status = true;
        CurrentFoodItem.IsCustom = true;
        
      

        _foodItemService.UpdateFoodItem(CurrentFoodItem);
      
        TempData["success"] = "Food updated successgfully!";
        return Redirect("/FoodTracker");
    }
}




        
       
        
