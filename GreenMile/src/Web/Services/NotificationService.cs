using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;
using Web.Utils;

namespace Web.Services;

/// <summary>
/// Service for sending notifications to users.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly DataContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(DataContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public DbSet<Notification> Notifications => _context.Notifications;

    public async Task<IEnumerable<Notification>> GetNotifications(User user)
    {
        return user is null
            ? throw new ArgumentNullException(nameof(user))
            : await _context.Notifications
                .Where(n => n.User == user)
                .ToListAsync();
    }

    public async Task SendNotification(Notification notification)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }
        var users = _context.Users.AsEnumerable();
        foreach (var user in users)
        {
            var notificationClone = new Notification()
            {
                Message = notification.Message,
                System = notification.System,
                Date = notification.Date,
                Read = notification.Read,
            };
            notificationClone.SetUser(user);
            _context.Notifications.Add(notificationClone);
        }
        await _context.SaveChangesAsync();
        await _hubContext.Clients.All.SendAsync(NotificationHub.ReceiveNotification, notification);
    }

    public async Task SendNotification(Notification notification, User user)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        notification.SetUser(user);
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        await _hubContext.Clients
            .User(user.Id)
            .SendAsync(NotificationHub.ReceiveNotification, notification);
        await _hubContext.Clients
            .User(user.Id)
            .SendAsync(
                NotificationHub.ReceiveNotificationCount,
                Notifications
                    .Where(n => n.Read == false)
                    .Count(n => n.User == user));
    }

    public async Task SendNotification(Notification notification, IEnumerable<User> users)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }
        if (users is null)
        {
            throw new ArgumentNullException(nameof(users));
        }
        foreach (var user in users)
        {
            var notificationClone = new Notification()
            {
                Message = notification.Message,
                System = notification.System,
                Date = notification.Date,
                Read = notification.Read,
            };
            notificationClone.SetUser(user);
            _context.Notifications.Add(notificationClone);
        }
        await _context.SaveChangesAsync();
        var userIdList = users.Select(u => u.Id).ToList();
        await _hubContext.Clients
            .Users(userIdList)
            .SendAsync(NotificationHub.ReceiveNotification, notification);

        foreach (var user in users)
        {
            await _hubContext.Clients
                .User(user.Id)
                .SendAsync(
                    NotificationHub.ReceiveNotificationCount,
                    Notifications
                        .Where(n => n.Read == false)
                        .Count(n => n.User == user));
        }
    }

    public async Task SendNotification(Notification notification, Household household)
    {
        if (notification is null)
        {
            throw new ArgumentNullException(nameof(notification));
        }
        if (household is null)
        {
            throw new ArgumentNullException(nameof(household));
        }
        if (household.Users is null)
        {
            throw new InvalidOperationException($"Member `Users` is `null` in argument {nameof(household)}.");
        }
        foreach (var user in household.Users)
        {
            var notificationClone = new Notification()
            {
                Message = notification.Message,
                System = notification.System,
                Date = notification.Date,
                Read = notification.Read,
            };
            notificationClone.SetUser(user);
            _context.Notifications.Add(notificationClone);
        }
        await _context.SaveChangesAsync();
        var userIdList = household.Users.Select(u => u.Id).ToList();
        await _hubContext.Clients
            .Users(userIdList)
            .SendAsync(NotificationHub.ReceiveNotification, notification);

        foreach (var user in household.Users)
        {
            await _hubContext.Clients
                .User(user.Id)
                .SendAsync(
                    NotificationHub.ReceiveNotificationCount,
                    Notifications
                        .Where(n => n.Read == false)
                        .Count(n => n.User == user));
        }
    }

    public async Task<Notification?> FindById(int id)
    {
        return await Notifications.FindAsync(id);
    }

    public async Task Update(Notification notification)
    {
        Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Notification notification)
    {
        Notifications.Remove(notification);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetUnreadCount(User user)
    {
        return await Notifications
            .Where(n => n.User == user)
            .CountAsync(n => n.Read == false);
    }
}