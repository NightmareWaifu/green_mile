using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Web.Models;
using Web.Services;

namespace Web.Utils
{
    public class ChatHub : Hub
    {
        private readonly UserManager<User> _userManager;
        private readonly ChatService _chatService;
        public ChatHub(UserManager<User> userManager, ChatService chatService)
        {
            _userManager = userManager;
            _chatService = chatService;
        }

        public async Task SendMessage(string userId, string text, int requestId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var message = await _chatService.CreateMessage(user, text, requestId);
            await Clients.Group(requestId.ToString()).SendAsync("ReceiveMessage", user.UserName, message.Id, text, message.Timestamp.ToShortTimeString());
        }

        public async Task DeleteMessage(string messageId)
        {
            var success = int.TryParse(messageId, out var messageIdInt);
            if (!success)
            {
                return;
            }
            var message = await _chatService.FindById(messageIdInt);
            if (message is not null)
            {
                var requestId = await _chatService.GetRequestId(message);
                await _chatService.DeleteMessage(message);
                await Clients.Group(requestId.ToString()).SendAsync("ReceiveDeleteMessage", messageId);
            }
        }

        public async Task JoinGroup(int requestId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, requestId.ToString());
        }
    }
}

