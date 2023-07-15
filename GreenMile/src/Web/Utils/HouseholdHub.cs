using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

using Web.Models;
using Web.Services;
using System;

namespace Web.Utils;

public class HouseholdHub : Hub
{
    private readonly UserManager<User> _userManager;
    private readonly MessageService _messageService;

    public class MessageViewModel
    {
        public int Key { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
        public string? ImagePath { get; set; }
    }

    public HouseholdHub(UserManager<User> userManager, MessageService messageService)
    {
        _userManager = userManager;
        _messageService = messageService;
    }

    private static string GetFileExtention(string file)
    {
        // e.g. data:image/png;base64
        var x = file.Split(';')[0];
        var y = x.Split('/')[1];
        return y;
    }

    public async Task SendMessageWithImage(string userId, string text, string imagePackage)
    {
        var imagePackageSplit = imagePackage.Split(',', 2);
        var base64String = imagePackageSplit[1];
        var fileExtention = GetFileExtention(imagePackageSplit[0]);
        byte[] bytes = Convert.FromBase64String(base64String);
        var imagePath = $"chatUploads/{Guid.NewGuid()}.{fileExtention}";
        using var fs = new FileStream($"wwwroot/{imagePath}", FileMode.Create);
        await fs.WriteAsync(bytes);
        var user = await _userManager.FindByIdAsync(userId);
        var message = await _messageService.CreateMessage(user, text, imagePath);
        await Clients.Group(user.HouseholdId.ToString()).SendAsync("ReceiveMessage", new MessageViewModel()
        {
            Key = message.Key,
            Username = user.UserName,
            Message = message.Text,
            ImagePath = message.ImagePath,
            CreatedAt = message.CreatedAt.ToShortTimeString()
        });
    }

    public async Task SendMessage(string userId, string text)
    {
        Console.WriteLine("Send message");
        var user = await _userManager.FindByIdAsync(userId);
        var message = await _messageService.CreateMessage(user, text);
        await Clients.Group(user.HouseholdId.ToString()).SendAsync("ReceiveMessage", new MessageViewModel()
        {
            Key = message.Key,
            Username = user.UserName,
            Message = message.Text,
            ImagePath = message.ImagePath,
            CreatedAt = message.CreatedAt.ToShortTimeString()
        });
    }

    public async Task DeleteMessage(string messageId)
    {
        Console.WriteLine("Tried to delete message");
        Console.WriteLine(messageId);
        var success = int.TryParse(messageId, out var messageIdInt);
        if (!success)
        {
            return;
        }
        var message = await _messageService.FindById(messageIdInt);
        if (message is not null)
        {
            var householdId = await _messageService.GetHouseholdId(message);
            await _messageService.DeleteMessage(message);
            await Clients.Group(householdId.ToString()).SendAsync("ReceiveDeleteMessage", messageId);
        }
    }

    public async Task JoinGroup(string householdId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, householdId);
    }
}