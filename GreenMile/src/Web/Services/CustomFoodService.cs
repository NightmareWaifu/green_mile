using Web.Data;
using Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.Services
{
    public class CustomFoodService
    {
        private readonly DataContext _context;

        public CustomFoodService(DataContext context)
        {
            _context = context;
        }

        public void AddCustomFood(CustomFood customfood)
        {
            _context.CustomFoods.Add(customfood);
            _context.SaveChanges();
        }
        public List<CustomFood> GetAll()
        {
            return _context.CustomFoods.OrderBy(m => m.Id).ToList();
        }
        public async Task<CustomFood?> GetCustomFoodById(int id)
        {
            return await _context.CustomFoods.FindAsync(id);
        }

        public async Task Update(CustomFood customfood)
        {
            _context.CustomFoods.Update(customfood);
            await _context.SaveChangesAsync();
        }

        public List<Donation> GetDonationsByUser(string id)
        {
            return _context.Donations
                .Include(d => d.CustomFood)
                .Where(x => x.User.Id.Equals(id))
                .OrderByDescending(m => m.Date)
                .ToList();
        }
    }
}
