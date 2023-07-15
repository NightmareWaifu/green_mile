using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Web.Lib;
using Web.Models;
using Web.Services;

namespace Web.Pages.FoodTracker
{
    public class AddFoodItemModel : PageModel
    {
        private readonly FoodItemService _fooditemService;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _http;
        private readonly IHouseholdService _householdService;
        private readonly FoodCategoryService _foodcategoryService;
        private IWebHostEnvironment _environment;

        public AddFoodItemModel(IHttpContextAccessor http, IHouseholdService householdService, FoodItemService fooditemService, IWebHostEnvironment environment, FoodCategoryService foodCategoryService,UserManager<User> userManager)
        {
            _fooditemService = fooditemService;
            _environment = environment;
            _foodcategoryService = foodCategoryService;
            _http = http;
            _householdService = householdService;
            _userManager = userManager;
        }
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
        public double Cost { get; set; }

        [BindProperty, Required]
        public double CarbonFoodprint { get;set;}

     
        //public string householdName { get; set; }


        public async Task OnGetAsync()
        {
            Categories = _foodcategoryService.GetAll();
            ExpiryDate = DateTime.Now;
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //var household = ( await _householdService.RetrieveHouseholdDetails(user.HouseholdId ?? -1)).Value;
            //householdName = household.Name;

        }



        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }



            if (ModelState.IsValid)
            {
                if (ImageFilePath != null)
                {
                    if (ImageFilePath.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(ImageFilePath.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await ImageFilePath.CopyToAsync(fileStream);
                    var ImageURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);

                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var household = (await _householdService.RetrieveHouseholdDetails(user.HouseholdId ?? -1)).Value;
                    //var householdName = _http.HttpContext?.Session.GetString(SessionVariable.HousholdName);
                    //var household = (await _householdService.RetrieveHouseholdDetailsByName(householdName)).Value;
                    //var category = _foodcategoryService.GetCategoryByName(Category);
                    var newfood = new FoodItem()
                    {
                        Household = household,
                        Name = Name,
                        Description = Description,
                        Quantity = Quantity,
                        ExpiryDate = ExpiryDate,
                        ImageFilePath = ImageURL,
                        Category = Category,
                        CarbonFootprint = 1.1,
                        Weight = 1.1,
                        Cost = Cost,
                        IsCustom = true,
                        Status = true
                    };

                    var newcat = new Category()
                    {
                        Name = "Fruit",
                        Description = "Healthy for the body"
                    };

                    _foodcategoryService.AddCategory(newcat);
                    _fooditemService.AddFoodItem(newfood);
                    TempData["success"] = "Food added successgfully!";
                    return Redirect("/FoodTracker");
                }

               

            }
            return Page();


           
        }
    }

}