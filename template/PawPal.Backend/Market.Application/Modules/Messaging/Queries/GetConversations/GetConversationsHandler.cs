using PawPal.Application.Modules.Messaging.Dtos;
using PawPal.Application.Modules.Messaging.Queries.GetConversations;

public sealed class GetConversationsQueryHandler(IAppDbContext context) :
    IRequestHandler<GetConversationsQuery, List<ConversationDto>>
{
    public async Task<List<ConversationDto>> Handle(GetConversationsQuery query, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Where(x => x.Id == query.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new PawPalNotFoundException("User does not exist");

        return await context.Conversations
            .Where(c => c.User1Id == query.UserId || c.User2Id == query.UserId)
            .Select(c => new ConversationDto
            {
                ConversationId = c.Id,
                OtherUserId = c.User1Id == query.UserId ? c.User2Id : c.User1Id,
                OtherUsername = c.User1Id == query.UserId
                    ? c.User2.Username
                    : c.User1.Username,
                LastMessage = c.Messages
                    .OrderByDescending(m => m.SentAt)
                    .Select(m => m.Content)
                    .FirstOrDefault(),
                LastMessageAt = c.Messages
                    .OrderByDescending(m => m.SentAt)
                    .Select(m => (DateTime?)m.SentAt)
                    .FirstOrDefault(),
                UnreadCount = c.Messages
                    .Count(m => m.SenderId != query.UserId && !m.IsRead)
            })
            .OrderByDescending(c => c.LastMessageAt)
            .ToListAsync(cancellationToken);
    }
}