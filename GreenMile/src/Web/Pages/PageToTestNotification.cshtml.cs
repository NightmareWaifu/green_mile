using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;

namespace Web.Pages;

public class PageToTestNotificationModel : PageModel
{
    private readonly INotificationService _notificationService;
    private readonly UserManager<User> _userManager;

    public PageToTestNotificationModel(INotificationService notificationService, UserManager<User> userManager)
    {
        _notificationService = notificationService;
        _userManager = userManager;
    }

    [BindProperty]
    public int Choice { get; set; }
    public async Task<IActionResult> OnPost()
    {
        var user = await _userManager.GetUserAsync(User);
        var notification = _notificationService.Create($"System {Choice}", $"Suck your mom {Choice}");
        await _notificationService.SendNotification(notification, user);
        return Page();
    }
}