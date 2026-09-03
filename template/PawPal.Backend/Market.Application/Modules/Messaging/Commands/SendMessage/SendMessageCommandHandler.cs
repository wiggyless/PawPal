using PawPal.Application.Abstractions;
using PawPal.Application.Modules.Messaging.Dtos;
using PawPal.Domain.Entities.Messaging;

namespace PawPal.Application.Modules.Messaging.Commands.SendMessage
{
    public sealed class SendMessageCommandHandler(IAppDbContext context, IMessageHubService messageHubService, IAppCurrentUser currentUser) :
        IRequestHandler<SendMessageCommand, MessageDto>
    {
        public async Task<MessageDto> Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            var senderId = currentUser.UserId ?? throw new PawPalConflictException("User is not authenticated");

            int u1 = Math.Min(senderId, command.RecipientId);
            int u2 = Math.Max(senderId, command.RecipientId);

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
                SenderId = senderId,
                Content = command.Content,
                SentAt = DateTime.UtcNow
            };

            context.Messages.Add(message);
            await context.SaveChangesAsync(cancellationToken);

            var sender = await context.Users
                .Where(x => x.Id == senderId)
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