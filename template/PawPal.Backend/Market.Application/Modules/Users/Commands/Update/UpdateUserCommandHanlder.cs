using PawPal.Application.Modules.Animal_Info.Animals.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.Update
{
    public sealed class UpdateUserCommandHanlder(IAppDbContext context)
        : IRequestHandler<UpdateUserCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateUserCommand request,CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if(user == null)
            {
                throw new PawPalNotFoundException($"User with Id {request.Id} does not exist!");
            }
            var city = await context.Cities.FirstOrDefaultAsync(x => x.Id == request.CityId,cancellationToken);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.ProfilePictureURL = request.ProfilePictureURL;
            user.BirthDate = request.Date;
            user.CityId = request.CityId;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
