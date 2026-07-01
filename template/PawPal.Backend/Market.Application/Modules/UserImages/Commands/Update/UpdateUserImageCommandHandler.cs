using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UserImages.Commands.Update
{
    public class UpdateUserImageCommandHandler(IAppDbContext context) : IRequestHandler<UpdateUserImageCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateUserImageCommand command,CancellationToken cancellationToken)
        {
            var userImage = await context.UserImage.Where(x => x.UserID == command.UserID).FirstOrDefaultAsync(cancellationToken);
            if (userImage is null)
            {
                throw new PawPalNotFoundException($"User with id {command.UserID} not found");
            }
            userImage.PhotoURL = "/users/User_" + command.UserID + "/" + command.Image.FileName;
            userImage.Name = command.Image.FileName;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
