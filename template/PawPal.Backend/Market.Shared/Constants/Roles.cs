namespace PawPal.Shared.Constants;

/// <summary>
/// Ids of the seeded rows in the Roles table (see DynamicDataSeeder.SeedRolesAsync).
/// Use these instead of comparing RoleId to a raw number.
/// </summary>
public static class Roles
{
    public const int BasicUser = 1;
    public const int VerifiedUser = 2;
    public const int Admin = 3;
}
