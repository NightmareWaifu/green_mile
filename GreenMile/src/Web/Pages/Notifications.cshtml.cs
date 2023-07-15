using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;

namespace Web.Pages;

[Authorize]
public class NotificationsModel : PageModel
{
    private readonly INotificationService _notificationService;
    private readonly UserManager<User> _userManager;

    public NotificationsModel(INotificationService notificationService, UserManager<User> userManager)
    {
        _notificationService = notificationService;
        _userManager = userManager;
    }

    public IEnumerable<Notification>? Notifications { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(HttpContext.User), "Member"))
        {
            return Redirect("/account/transferhousehold");
        }
        var user = await _userManager.GetUserAsync(HttpContext.User);
        Notifications = await _notificationService.GetNotifications(user);
        return Page();
    }

    public async Task<IActionResult> OnPostReadAll()
    {
        var user = await _userManager.GetUserAsync(User);
        var notifications = await _notificationService.GetNotifications(user);
        foreach (var notif in notifications)
        {
            notif.Read = true;
            await _notificationService.Update(notif);
        }
        Notifications = await _notificationService.GetNotifications(user);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteRead()
    {
        var user = await _userManager.GetUserAsync(User);
        var notifications = await _notificationService.GetNotifications(user);
        foreach (var notif in notifications)
        {
            if (notif.Read)
            {
                await _notificationService.Delete(notif);
            }
        }
        Notifications = await _notificationService.GetNotifications(user);
        return Page();
    }
}