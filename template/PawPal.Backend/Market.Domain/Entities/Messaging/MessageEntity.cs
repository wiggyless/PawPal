using PawPal.Domain.Common;
using PawPal.Domain.Entities.Identity;

public class MessageEntity : BaseEntity
{
    public int ConversationId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    public ConversationEntity Conversation { get; set; } = null!;
    public UserEntity Sender { get; set; } = null!;
}