using Microsoft.AspNetCore.Http;
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UserImages.Commands.Create
{
    public class CreateUserImageCommandHandler(IAppDbContext context) : IRequestHandler<CreateUserImageCommand, int>
    {
        public async Task<int> Handle(CreateUserImageCommand command,CancellationToken cancellationToken)
        {
            var user = await context.Users.Where(x => x.Id == command.UserID).FirstOrDefaultAsync(cancellationToken);
            if (user is null)
                throw new PawPalNotFoundException($"Post with ID:{command.UserID} not found");

            var newPostImages = new UserImage
            {
                UserID = command.UserID,
                PhotoURL = "/users/User_" + command.UserID + "/" + command.Image.FileName,
                Name = command.Image.FileName,
            };
            context.UserImage.Add(newPostImages);
            await context.SaveChangesAsync(cancellationToken);
            return newPostImages.Id;
        }
    }
}
