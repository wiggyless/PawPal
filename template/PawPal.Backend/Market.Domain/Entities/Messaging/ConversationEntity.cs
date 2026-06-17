using PawPal.Domain.Common;
using PawPal.Domain.Entities.Identity;

public class ConversationEntity : BaseEntity
{
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    public ICollection<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    public UserEntity User1 { get; set; } = null!;  
    public UserEntity User2 { get; set; } = null!;
}