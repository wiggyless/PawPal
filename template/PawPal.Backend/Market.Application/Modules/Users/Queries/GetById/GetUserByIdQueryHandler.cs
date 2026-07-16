using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetById
{
    public sealed class GetUserByIdQueryHandler(IAppDbContext context,IAppCurrentUser currUser)
        : IRequestHandler<GetUserByIdQuery,GetUserByIdQueryDto>
    {
        public async Task<GetUserByIdQueryDto> Handle(GetUserByIdQuery request,CancellationToken cancellationToken)
        {
            var userImage = await context.UserImage.FirstOrDefaultAsync(x => x.UserID == request.Id, cancellationToken);
            if(userImage is null)
            {
                userImage = new UserImage
                {
                    Id = 0,
                    UserID = request.Id,
                    PhotoURL = "",
                };
            }
            var user = await context.Users.
                Include(x =>x.City).
                Include(x=>x.City.Canton).
                Where(a => a.Id == request.Id && (currUser.RoleId == 3 ? true : !a.isUserDisabled)).
                Select(x => new GetUserByIdQueryDto
                {
     
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    DateTime = x.BirthDate,
                    City = x.City.Name,
                    CantonAbbrevation = x.City.Canton.Abbreviation,
                    CityID = x.CityId,
                    Username = x.Username,
                    AboutMe = x.AboutMe,
                    PhotoURL =userImage.PhotoURL,
                    Disabled = x.isUserDisabled,
                }).FirstOrDefaultAsync(cancellationToken);
            if (user == null) throw new PawPalNotFoundException($"User with Id {request.Id} is either disabled or deleted");
            return user;
        }
    }
}
