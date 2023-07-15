using Web.Data;
using Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.Services
{
    public class DonationRequestService
    {
        private readonly INotificationService _notificationService;
        private readonly DataContext _context;

        public DonationRequestService(DataContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task AddRequest(DonationRequest request)
        {
            _context.DonationRequests.Add(request);
            _context.SaveChanges();
            var notif = _notificationService.Create("Food Sharing", $"{request.Recipient.UserName} requests for {request.Donation.CustomFood.Name}");
            await _notificationService.SendNotification(notif, request.Donor);
        }
        public List<DonationRequest> GetAll()
        {
            return _context.DonationRequests.OrderBy(m => m.Id).ToList();
        }

        public async Task Update(DonationRequest request)
        {
            _context.DonationRequests.Update(request);
            await _context.SaveChangesAsync();
        }
        public List<DonationRequest> GetRequestByDonor(string userId)
        {
            return _context.DonationRequests
                .Include(d => d.Donation.CustomFood)
                .Where(x => x.Donor.Id == userId && x.Status != RequestStatus.CANCELLED)
                .ToList();
        }

        public async Task<bool> DonationRequestExists(int donationId, string userId)
        {
            var request = await _context.DonationRequests
                .Where(x => x.Donation.Id == donationId && x.Recipient.Id == userId)
                .FirstOrDefaultAsync();

            if (request == null) { return false; } else { return true; }

        }


        public List<DonationRequest> GetRequestsByRecipient(string id)
        {
            return _context.DonationRequests
                .Include(d => d.Donation.CustomFood)
                .Where(x => x.Recipient.Id.Equals(id))
                .OrderByDescending(m => m.Date)
                .ToList();
        }

        public async Task<DonationRequest> GetRequestsByRequestID(int id)
        {
            return await _context.DonationRequests
                .Include(d => d.Donation.CustomFood)
                .Where(x => x.Id.Equals(id))
                .FirstAsync();
        }
    }
}
