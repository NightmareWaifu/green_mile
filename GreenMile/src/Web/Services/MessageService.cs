using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;

namespace Web.Services;

public class MessageService
{
    private readonly DataContext _context;
    private readonly IHouseholdService _householdService;
    private readonly IHostEnvironment _environment;

    public MessageService(DataContext context, IHouseholdService householdService, IHostEnvironment environment)
    {
        _context = context;
        _householdService = householdService;
        _environment = environment;
    }

    public DbSet<Message> Messages => _context.Messages;

    public async Task<Message> CreateMessage(User user, string text, string? filePath = null)
    {
        var household = (await _householdService.RetrieveHouseholdDetails((int)user.HouseholdId)).Value;
        var message = new Message
        {
            User = user,
            Household = household,
            Text = text,
            ImagePath = filePath
        };
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<Message>> GetMessagesByHouseholdId(int householdId)
    {
        var household = (await _householdService.RetrieveHouseholdDetails(householdId)).Value;

        return await _context.Messages
            .Where(m => m.Household == household)
            .ToListAsync();
    }

    public async Task<Message?> FindById(int id)
    {
        return await Messages.FindAsync(id);
    }

    public async Task DeleteMessage(Message message)
    {
        if (message.ImagePath is not null)
        {
            try
            {
                File.Delete(_environment.ContentRootPath + "wwwroot/" + message.ImagePath);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }

        }
        Messages.Remove(message);
        await _context.SaveChangesAsync();
    }

    public async Task<int?> GetHouseholdId(Message message)
    {
        return await Messages
            .Include(m => m.Household)
            .Where(m => m.Key == message.Key)
            .Select(m => m.Household.HouseholdId)
            .FirstOrDefaultAsync();
    }
}