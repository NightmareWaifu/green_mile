using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DataContext _context;

        public ChatHub(DataContext context)
        {
            _context = context;
        }

       
    }
}
