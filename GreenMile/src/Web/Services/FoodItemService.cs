

using Web.Data;
using Web.Models;

namespace Web.Services
{
    public class FoodItemService
    {
        private readonly DataContext _context;

        public FoodItemService(DataContext context)
        {
            _context = context;
        }

        public List<FoodItem> GetAllForRecipe()
        {
            return _context.FoodItems.ToList();
        }

        public List<FoodItem> GetAll(Household household)

        {


            return _context.FoodItems.Where(x => x.Household.Equals(household)).ToList();

        }

        public async Task<FoodItem?> GetFoodItemById(int id)
        {


            return _context.FoodItems.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void AddFoodItem(FoodItem fooditem)
        {
            _context.FoodItems.Add(fooditem);
            _context.SaveChanges();
        }


        public void UpdateFoodItem(FoodItem fooditem)
        {
            _context.FoodItems.Update(fooditem);
            _context.SaveChanges();
        }

        public void DeleteFoodItem(FoodItem fooditem)
        {
            _context.FoodItems.Remove(fooditem);
            _context.SaveChanges();
        }


    }
}