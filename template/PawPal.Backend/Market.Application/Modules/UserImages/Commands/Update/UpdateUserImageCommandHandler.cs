using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UserImages.Commands.Update
{
    public class UpdateUserImageCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<UpdateUserImageCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateUserImageCommand command,CancellationToken cancellationToken)
        {
            if (command.UserID != currentUser.UserId && currentUser.RoleId != 3)
                throw new PawPalConflictException("User is not allowed to do this action");

            var userImage = await context.UserImage.Where(x => x.UserID == command.UserID).FirstOrDefaultAsync(cancellationToken);
            if (userImage is null)
            {
                throw new PawPalNotFoundException($"User with id {command.UserID} not found");
            }
            var safeFileName = Path.GetFileName(command.Image.FileName);
            userImage.PhotoURL = "/users/User_" + command.UserID + "/" + safeFileName;
            userImage.Name = safeFileName;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
