using Web.Data;
using Web.Utils;
using Web.Models;
namespace Web.Services
{
    public interface IGroceryFoodService
    {


        public Task<Boolean> Add(GroceryFoodItem groceryItem);

        public Task ChangeQuantity(GroceryFoodItem groceryItem, int quantity);




        public Task Delete(string id);

        public  Task<GroceryFoodItem> RetrieveGroceryItem(string id);

        public  Task<Result<ICollection<GroceryFoodItem>>> RetrieveFoodByHousehold(int householdId);
        public Task<string> ExportGroceryList(int householdId);
        public Task ImportGroceryList(int householdId, string json);

    }
}
