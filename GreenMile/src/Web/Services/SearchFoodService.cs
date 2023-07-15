using Web.Data;
using Web.Models;

namespace Web.Services
{
    public class SearchFoodService
    {


        private readonly DataContext _context;

        public SearchFoodService(DataContext context)
        {
            _context = context;
        }

        //public List<SearchFoodItem> GetAll()
        //{


        //    return _context.SearchFoodItems.ToList();

        //}

        //public async Task<SearchFoodItem?> GetFoodItemById(int id)
        //{


        //    return _context.SearchFoodItems.FirstOrDefault(x => x.Id.Equals(id));
        //}

        //public void AddFoodItem(SearchFoodItem fooditem)
        //{
        //    _context.SearchFoodItems.Add(fooditem);
        //    _context.SaveChanges();
        //}
        //public void DeleteAll(SearchFoodItem fooditem)
        //{
        //    var fooditems = _context.SearchFoodItems.ToList();
        //    foreach (var foodItem in fooditems)
        //    {
        //        _context.SearchFoodItems.Remove(foodItem);
        //    }
        //    _context.SearchFoodItems.Remove(fooditem);
        //    _context.SaveChanges();
        //}
    }
}
