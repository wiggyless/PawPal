using PawPal.Application.Abstractions;
using PawPal.Application.Modules.Messaging.Dtos;
using PawPal.Domain.Entities.Messaging;

namespace PawPal.Application.Modules.Messaging.Commands.SendMessage
{
    public sealed class SendMessageCommandHandler(IAppDbContext context, IMessageHubService messageHubService) :
        IRequestHandler<SendMessageCommand, MessageDto>
    {
        public async Task<MessageDto> Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            int u1 = Math.Min(command.SenderId, command.RecipientId);
            int u2 = Math.Max(command.SenderId, command.RecipientId);

            var conversation = await context.Conversations
                .FirstOrDefaultAsync(c => c.User1Id == u1 && c.User2Id == u2, cancellationToken);

            if (conversation is null)
            {
                conversation = new ConversationEntity { User1Id = u1, User2Id = u2 };
                context.Conversations.Add(conversation);
                await context.SaveChangesAsync(cancellationToken);
            }

            var message = new MessageEntity
            {
                ConversationId = conversation.Id,
                SenderId = command.SenderId,
                Content = command.Content,
                SentAt = DateTime.UtcNow
            };

            context.Messages.Add(message);
            await context.SaveChangesAsync(cancellationToken);

            var sender = await context.Users
                .Where(x => x.Id == command.SenderId)
                .FirstOrDefaultAsync(cancellationToken);

            if (sender is null)
                throw new PawPalNotFoundException("Sender does not exist");

            var dto = new MessageDto
            {
                MessageId = message.Id,
                ConversationId = conversation.Id,
                SenderId = message.SenderId,
                SenderUsername = sender.Username ?? string.Empty,
                Content = message.Content,
                SentAt = message.SentAt,
                IsRead = false
            };

            await messageHubService.SendMessageToUser(command.RecipientId, dto);

            return dto;
        }
    }
}