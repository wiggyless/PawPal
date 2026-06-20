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
            Console.WriteLine(currUser);
            if(currUser.UserId != request.Id && currUser.IsAuthenticated! && currUser.RoleId != 3)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var user = await context.Users.
                Include(x =>x.City).
                Include(x=>x.City.Canton).
                Where(a => a.Id == request.Id).
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
                    AboutMe = x.AboutMe
                }).FirstOrDefaultAsync(cancellationToken);
            if (user == null) throw new PawPalNotFoundException($"User with Id {request.Id} does not exist");
            return user;
        }
    }
}
