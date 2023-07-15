using Microsoft.AspNetCore.SignalR;

using Web.Models;
using Web.Services;

namespace Web.Utils;

public class NotificationHub : Hub
{
    private readonly INotificationService _notificationService;

    public NotificationHub(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public const string ReceiveNotification = "ReceiveNotification";
    public const string ReceiveNotificationCount = "ReceiveNotificationCount";

    public async Task ReadNotification(int notificationId)
    {
        Notification? notification = await _notificationService.FindById(notificationId);
        if (notification is null)
            return;

        notification.Read = true;
        await _notificationService.Update(notification);
    }

    public async Task DeleteNotification(int notificationId)
    {
        Console.WriteLine($"TRIED TO DELETE {notificationId}");
        var notification = await _notificationService.FindById(notificationId);
        if (notification is not null)
        {
            Console.WriteLine("SUCCESS");
            await _notificationService.Delete(notification);
        }
    }
}