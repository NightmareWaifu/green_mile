using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json;

using Web.Data;
using Web.Lib;
using Web.Models;
using Web.Services;

namespace Web.Pages.FoodTracker
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly FoodItemService _fooditemService;
        private readonly UserManager<User> _userManager;


        private readonly FoodCategoryService _foodcategoryService;

        private readonly IHouseholdService _householdService;
        private readonly IHttpContextAccessor _contextAccessor;

       



        public IndexModel(FoodItemService fooditemService, IHouseholdService householdService, IHttpContextAccessor contextAccessor, FoodCategoryService foodcategoryService, UserManager<User> userManager)

        {
            _fooditemService = fooditemService;
            _userManager = userManager;
            _householdService = householdService;

            _contextAccessor = contextAccessor;

            _foodcategoryService = foodcategoryService;


        }
        public List<FoodItem> FoodItemList { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string action_type { get; set; }

       


        public int Count { get; set; }
        public string Name { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var household = (await _householdService.RetrieveHouseholdDetails(user.HouseholdId ?? -1)).Value;
            FoodItemList = _fooditemService.GetAll(household);
            Name = household.Name;
            Count = FoodItemList.Count();

         

        }
       
        public async Task<IActionResult> OnPostAsync()
        {

            FoodItem? food = await _fooditemService.GetFoodItemById(Id);

            if (food != null)
            {
                if(action_type == "delete")
                {
                    _fooditemService.DeleteFoodItem(food);
                    return Redirect("/FoodTracker");
                }
                
            }
            else
            {
                return Page();
            }

            return Redirect("/FoodTracker");

 
       


        }
    }
}
