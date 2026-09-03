namespace PawPal.Application.Modules.Users.Queries.GetPublicProfile
{
    public class GetPublicUserProfileQuery : IRequest<GetPublicUserProfileQueryDto>
    {
        public int Id { get; set; }
    }
}
