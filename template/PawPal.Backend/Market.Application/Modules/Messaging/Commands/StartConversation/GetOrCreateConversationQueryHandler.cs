using PawPal.Application.Modules.Messaging.Commands.StartConversation;
using PawPal.Application.Modules.Messaging.Dtos;

public sealed class GetOrCreateConversationQueryHandler(IAppDbContext context) :
    IRequestHandler<GetOrCreateConversationQuery, ConversationDto>
{
    public async Task<ConversationDto> Handle(GetOrCreateConversationQuery query, CancellationToken cancellationToken)
    {
        int u1 = Math.Min(query.SenderId, query.RecipientId);
        int u2 = Math.Max(query.SenderId, query.RecipientId);

        var conversation = await context.Conversations
    .FirstOrDefaultAsync(c => c.User1Id == u1 && c.User2Id == u2, cancellationToken);

        if (conversation is null)
        {
            conversation = new ConversationEntity { User1Id = u1, User2Id = u2 };
            context.Conversations.Add(conversation);
            await context.SaveChangesAsync(cancellationToken);
        }

        var otherUser = await context.Users
            .Where(x => x.Id == query.RecipientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (otherUser is null)
            throw new PawPalNotFoundException("User does not exist");



        return new ConversationDto
        {
            ConversationId = conversation.Id,
            OtherUserId = query.RecipientId,
            OtherUsername = otherUser.Username,
            LastMessage = null,
            LastMessageAt = null,
            UnreadCount = 0
        };
    }
}