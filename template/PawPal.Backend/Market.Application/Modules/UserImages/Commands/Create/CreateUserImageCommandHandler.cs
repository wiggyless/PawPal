using Microsoft.AspNetCore.Http;
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UserImages.Commands.Create
{
    public class CreateUserImageCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<CreateUserImageCommand, int>
    {
        public async Task<int> Handle(CreateUserImageCommand command,CancellationToken cancellationToken)
        {
            var user = await context.Users.Where(x => x.Id == command.UserID).FirstOrDefaultAsync(cancellationToken);
            if (user is null)
                throw new PawPalNotFoundException($"User with ID:{command.UserID} not found");

            // This is called anonymously right after registration, before the user has logged in,
            // so we can't require currentUser to match. Only allow the anonymous path for a brand
            // new account (within its signup grace window) that doesn't have an image yet — past
            // that, it must be the account owner (or an admin), which also blocks attackers from
            // planting a photo on someone else's older, still-photo-less account.
            var isOwnerOrAdmin = command.UserID == currentUser.UserId || currentUser.RoleId == 3;
            if (!isOwnerOrAdmin)
            {
                var alreadyHasImage = await context.UserImage.AnyAsync(x => x.UserID == command.UserID, cancellationToken);
                var withinSignupGracePeriod = user.CreatedAtUtc >= DateTime.UtcNow.AddMinutes(-15);
                if (alreadyHasImage || !withinSignupGracePeriod)
                    throw new PawPalConflictException("User is not allowed to do this action");
            }

            var safeFileName = Path.GetFileName(command.Image.FileName);
            var newPostImages = new UserImage
            {
                UserID = command.UserID,
                PhotoURL = "/users/User_" + command.UserID + "/" + safeFileName,
                Name = safeFileName,
            };
            context.UserImage.Add(newPostImages);
            await context.SaveChangesAsync(cancellationToken);
            return newPostImages.Id;
        }
    }
}
