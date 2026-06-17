
using PawPal.Application.Modules.Messaging.Dtos;

namespace PawPal.Application.Abstractions
{
    public interface IMessageHubService
    {
        Task SendMessageToUser(int recipientId, MessageDto message);
    }
}