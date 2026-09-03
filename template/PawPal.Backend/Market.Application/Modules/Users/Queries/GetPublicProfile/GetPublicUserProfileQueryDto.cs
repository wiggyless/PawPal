namespace PawPal.Application.Modules.Users.Queries.GetPublicProfile
{
    // Deliberately excludes Email, BirthDate, exact Canton, and Disabled status — those are only
    // ever returned by GetUserByIdQuery, which is restricted to the account owner or an admin.
    public class GetPublicUserProfileQueryDto
    {
        public int Id { get; set; }
        public required string? FirstName { get; set; }
        public required string? LastName { get; set; }
        public required string? Username { get; set; }
        public required string? City { get; set; }
        public int? CityID { get; set; }
        public required string? AboutMe { get; set; }
        public string? PhotoURL { get; set; }
    }
}
