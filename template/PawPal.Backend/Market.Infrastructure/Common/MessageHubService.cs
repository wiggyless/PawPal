using Microsoft.AspNetCore.SignalR;
using PawPal.Application.Abstractions;
using PawPal.Application.Modules.Messaging.Dtos;
using PawPal.Infrastructure.Signal;

public class MessageHubService : IMessageHubService
{
    private readonly IHubContext<MessageHub> _hubContext;

    public MessageHubService(IHubContext<MessageHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendMessageToUser(int recipientId, MessageDto message)
    {
        // Sends only to the recipient's personal group
        await _hubContext.Clients
            .Group($"user_{recipientId}")
            .SendAsync("ReceiveMessage", message);
    }
}