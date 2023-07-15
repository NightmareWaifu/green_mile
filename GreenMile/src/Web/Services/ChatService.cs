using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;

namespace Web.Services
{
    public class ChatService
    {
        private readonly DataContext _context;
        private readonly DonationRequestService _donationRequestService;

        public ChatService(DataContext context, DonationRequestService donationRequestService)
        {
            _context = context;
            _donationRequestService = donationRequestService;
        }

        public DbSet<MessageHistory> ChatMessages => _context.ChatMessages;

        public async Task<MessageHistory> CreateMessage(User user, string text, int requestId)
        {
            var donationRequest = await _donationRequestService.GetRequestsByRequestID(requestId);
            var message = new MessageHistory
            {
                User = user,
                DonationRequest = donationRequest,
                Text = text,
            };
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
        public async Task FirstMessage(MessageHistory message)
        {
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MessageHistory>> GetMessagesByRequestId(int requestId)
        {
            var donationRequest = await _donationRequestService.GetRequestsByRequestID(requestId);

            return await _context.ChatMessages
                .Include(m => m.User)
                .Where(m => m.DonationRequest == donationRequest)
                .ToListAsync();
        }

        public async Task<MessageHistory?> FindById(int id)
        {
            return await ChatMessages.FindAsync(id);
        }

        public async Task DeleteMessage(MessageHistory message)
        {
            ChatMessages.Remove(message);
            await _context.SaveChangesAsync();
        }
        public async Task<int?> GetRequestId(MessageHistory message)
        {
            return await ChatMessages
                .Include(m => m.DonationRequest)
                .Where(m => m.Id == message.Id)
                .Select(m => m.DonationRequest.Id)
                .FirstOrDefaultAsync();
        }
    }
}
