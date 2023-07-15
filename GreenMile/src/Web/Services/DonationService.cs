using System;

using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;

namespace Web.Services
{
    public class DonationService
    {
        private readonly DataContext _context;

        public DonationService(DataContext context)
        {
            _context = context;
        }

        // Only retrieve the donations that is not belong to the current user, and only retrieve Active Donation
        public List<Donation> GetAll(string id)
        {
            return _context.Donations
                .Include(d => d.CustomFood)
                .Include(d => d.User)
                .Where(d => !d.User.Id.Equals(id) && d.Status.Equals(DonationStatus.ACTIVE))
                .OrderBy(m => m.Id)
                .ToList();
        }

        public void AddDonation(Donation donation)
        {
            _context.Donations.Add(donation);
            _context.SaveChanges();
        }

        public async Task UpdateDonation(Donation donation)
        {
            _context.Donations.Update(donation);
            await _context.SaveChangesAsync();
        }

        public Donation? GetDonationById(int id)
        {

            return _context.Donations
                .Include(d => d.CustomFood)
                .Include(d => d.User)
                .FirstOrDefault(x => x.Id.Equals(id)); 
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
