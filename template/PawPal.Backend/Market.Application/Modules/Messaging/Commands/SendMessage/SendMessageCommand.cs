using PawPal.Application.Modules.Messaging.Dtos;

namespace PawPal.Application.Modules.Messaging.Commands.SendMessage
{
    public class SendMessageCommand : IRequest<MessageDto>
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
