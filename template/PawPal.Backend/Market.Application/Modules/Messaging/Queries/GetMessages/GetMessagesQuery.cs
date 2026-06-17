using PawPal.Application.Modules.Messaging.Dtos;

public class GetMessagesQuery : BasePagedQuery<MessageDto>
{
    public int ConversationId { get; set; }
    public int RequestingUserId { get; set; }
}