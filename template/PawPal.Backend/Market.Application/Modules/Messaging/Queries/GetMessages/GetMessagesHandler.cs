using PawPal.Application.Modules.Messaging.Dtos;

public sealed class GetMessagesQueryHandler(IAppDbContext context) :
    IRequestHandler<GetMessagesQuery, PageResult<MessageDto>>
{
    public async Task<PageResult<MessageDto>> Handle(GetMessagesQuery query, CancellationToken cancellationToken)
    {
        var conversation = await context.Conversations
            .Where(x => x.Id == query.ConversationId)
            .FirstOrDefaultAsync(cancellationToken);

        if (conversation is null)
            throw new PawPalNotFoundException("Conversation does not exist");

        var unread = await context.Messages
            .Where(m => m.ConversationId == query.ConversationId
                     && m.SenderId != query.RequestingUserId
                     && !m.IsRead)
            .ToListAsync(cancellationToken);

        if (unread.Any())
        {
            unread.ForEach(m => m.IsRead = true);
            await context.SaveChangesAsync(cancellationToken);
        }

        var messagesQuery = context.Messages
            .Where(m => m.ConversationId == query.ConversationId)
            .OrderByDescending(m => m.SentAt)
            .Select(m => new MessageDto
            {
                MessageId = m.Id,
                ConversationId = m.ConversationId,
                SenderId = m.SenderId,
                SenderUsername = m.Sender.Username,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            });

        return await PageResult<MessageDto>.FromQueryableAsync(messagesQuery, query.Paging, cancellationToken);
    }
}