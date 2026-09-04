using PawPal.Shared.Constants;
namespace PawPal.Application.Modules.Users.Queries.GetPublicProfile
{
    public sealed class GetPublicUserProfileQueryHandler(IAppDbContext context, IAppCurrentUser currUser)
        : IRequestHandler<GetPublicUserProfileQuery, GetPublicUserProfileQueryDto>
    {
        public async Task<GetPublicUserProfileQueryDto> Handle(GetPublicUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userImage = await context.UserImage.FirstOrDefaultAsync(x => x.UserID == request.Id, cancellationToken);
            if (userImage is null)
            {
                userImage = new UserImage
                {
                    Id = 0,
                    UserID = request.Id,
                    PhotoURL = "",
                };
            }
            var user = await context.Users.
                Include(x => x.City).
                Where(a => a.Id == request.Id && (currUser.RoleId == Roles.Admin ? true : !a.isUserDisabled)).
                Select(x => new GetPublicUserProfileQueryDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    City = x.City.Name,
                    CityID = x.CityId,
                    Username = x.Username,
                    AboutMe = x.AboutMe,
                    PhotoURL = userImage.PhotoURL,
                }).FirstOrDefaultAsync(cancellationToken);
            if (user == null) throw new PawPalNotFoundException($"User with Id {request.Id} is either disabled or deleted");
            return user;
        }
    }
}
