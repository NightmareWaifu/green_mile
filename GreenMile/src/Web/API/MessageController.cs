using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Web.Models;
using Web.Services;

namespace Web.API;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly MessageService _messageService;

    public MessageController(UserManager<User> userManager, MessageService messageService)
    {
        _userManager = userManager;
        _messageService = messageService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage(string userId, string text)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return NotFound();
        await _messageService.CreateMessage(user, text);
        return Ok();
    }
}