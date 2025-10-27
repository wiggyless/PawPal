﻿// MarketUserEntity.cs
using PawPal.Domain.Common;

namespace PawPal.Domain.Entities.Identity;

public sealed class MarketUserEntity : BaseEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsManager { get; set; }
    public bool IsEmployee { get; set; }
    public int TokenVersion { get; set; } = 0;// For global revocation
    public bool IsEnabled { get; set; }
    public ICollection<RefreshTokenEntity> RefreshTokens { get; private set; } = new List<RefreshTokenEntity>();
}