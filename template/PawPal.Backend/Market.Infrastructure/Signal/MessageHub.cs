using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PawPal.Infrastructure.Signal
{
    [Authorize]
    public class MessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task UserTyping(int recipientId)
        {
            await Clients.Group($"user_{recipientId}")
                         .SendAsync("UserTyping", Context.UserIdentifier);
        }

        public async Task UserStoppedTyping(int recipientId)
        {
            await Clients.Group($"user_{recipientId}")
                         .SendAsync("UserStoppedTyping", Context.UserIdentifier);
        }
    }
}