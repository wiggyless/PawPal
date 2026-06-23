using PawPal.Application.Modules.Animal_Info.Animals.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.Update
{
    public sealed class UpdateUserCommandHanlder(IAppDbContext context,IAppCurrentUser currentUser)
        : IRequestHandler<UpdateUserCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateUserCommand request,CancellationToken cancellationToken)
        {
            
            var user = await context.Users.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if(user == null)
            {
                throw new PawPalNotFoundException($"User with Id {request.Id} does not exist!");
            }
            if(currentUser.UserId != request.Id)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var city = await context.Cities.FirstOrDefaultAsync(x => x.Id == request.CityId,cancellationToken);
            if (!request.AreStringPropertiesValid("ProfilePictureURL","AboutMe"))
            {
                throw new PawPalConflictException($"Field cannot be an empty string");
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.ProfilePictureURL = request.ProfilePictureURL;
            user.BirthDate = request.Date;
            user.CityId = request.CityId;
            user.AboutMe = request.AboutMe;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
