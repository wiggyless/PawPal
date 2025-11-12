// MarketUserEntity.cs
using PawPal.Domain.Common;
using PawPal.Domain.Entities.Places;
using System.Security.Principal;

namespace PawPal.Domain.Entities.Identity;

public sealed class UserEntity : BaseEntity
{
    public string ?FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? ProfilePictureURL { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public bool OnlineStatus { get; set; }
    public int TokenVersion { get; set; } = 0;// For global revocation
    public bool IsEnabled { get; set; }
    public int ?CityId { get; set; }
    public CitiesEntity ?City { get; set; }
    public int ?RoleId { get; set; }
    public RolesEntity ?Role { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsManager { get; set; }
    public bool IsEmployee { get; set; }
    public ICollection<RefreshTokenEntity> RefreshTokens { get; private set; } = new List<RefreshTokenEntity>();
}