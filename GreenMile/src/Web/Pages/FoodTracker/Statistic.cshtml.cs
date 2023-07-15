using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;

namespace Web.Pages.FoodTracker
{
    public class StatisticModel : PageModel
    {
        private readonly FoodItemService _fooditemService;
        private readonly UserManager<User> _userManager;


        private readonly FoodCategoryService _foodcategoryService;

        private readonly IHouseholdService _householdService;
        private readonly IHttpContextAccessor _contextAccessor;

        public StatisticModel(FoodItemService fooditemService, IHouseholdService householdService, IHttpContextAccessor contextAccessor, FoodCategoryService foodcategoryService, UserManager<User> userManager)

        {
            _fooditemService = fooditemService;
            _userManager = userManager;
            _householdService = householdService;

            _contextAccessor = contextAccessor;

            _foodcategoryService = foodcategoryService;


        }

        public List<FoodItem> FoodItemList { get; set; }
        public List<FoodExpiry> Expirylist { get; set; } = new();
        public FoodExpiry Meat { get; set; } = new();
        public FoodExpiry Vegetable { get; set; } = new();
        public FoodExpiry Fruit { get; set; } = new();
        public FoodExpiry Dairy { get; set; } = new();
        public List<RecentActivity> Activitylist { get; set; } = new();
        public RecentActivity Activity { get; set; } = new();
        public double Inventory_cost { get; set; }
        public double Waste_cost { get; set; }


        public int Count { get; set; }
        public string Name { get; set; }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var household = (await _householdService.RetrieveHouseholdDetails(user.HouseholdId ?? -1)).Value;
            FoodItemList = _fooditemService.GetAll(household);
            // weight & cost of each category
            foreach (var food in FoodItemList)
            {
                if (food.ExpiryDate <= DateTime.Now)
                { 
                    if(food.Category == "Fruit")
                    {
                        Fruit.Category = "Fruit";
                        Fruit.Cost += 10;
                        Fruit.Weight += food.Weight;
                    }
                    if (food.Category == "Vegetable")
                    {
                        
                        Vegetable.Category = "Vegetable";
                        Vegetable.Cost += 10;
                        Vegetable.Weight += food.Weight;
                    }
                    if (food.Category == "Meat")
                    {
                        Meat.Category = "Meat";
                        Meat.Cost += 10;
                        Meat.Weight += food.Weight;
                    }
                    if (food.Category == "Dairy")
                    {
                        Dairy.Category = "Dairy";
                        Dairy.Cost += 10;
                        Dairy.Weight += food.Weight;
                    }
                    Activity.Name = food.Name;
                    Activity.Date = DateTime.Now.ToString();
                    Activity.Type = "Expired";
                    Activitylist.Add(Activity);
                    // cost of expired food
                    Waste_cost += food.Cost;
                }
                else if (food.ExpiryDate >= DateTime.Now){
                    // cost of current inventory
                    Inventory_cost += food.Cost;

                    //recent activity
                    Activity.Name = food.Name;
                    Activity.Date = food.ExpiryDate.ToString();
                    Activity.Type = "New";
                  
                    Activitylist.Add(Activity);
                    
                }
               
            }
            Expirylist.Add(Meat);
            Expirylist.Add(Vegetable);
            Expirylist.Add(Fruit);
            Expirylist.Add(Dairy);

            // cost of current inventory
            // cost of expired food
            // recent activity
            // recommendation


            Name = household.Name;
            Count = 0;
            foreach(var food in FoodItemList)
            {
                if(food.ExpiryDate < DateTime.Now)
                {
                    Count += 1;
                }
            }
            


        }
    }
}
