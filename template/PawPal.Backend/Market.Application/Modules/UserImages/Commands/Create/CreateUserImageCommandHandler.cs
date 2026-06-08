using Microsoft.AspNetCore.Http;
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
            var user = await context.Users.Where(x => x.Id == command.UserID).AsNoTracking().FirstOrDefaultAsync();
            if(user is null)
            {
                throw new PawPalNotFoundException("User does not exist inside the databse");
            }
            var file = command.Image;
            if (file is null || file.Length == 0) {
                throw new PawPalNotFoundException("Image has not been sent properly");
            }
            using (var memoryStream = new MemoryStream()) { 
                await file.CopyToAsync(memoryStream);
                byte[] data = memoryStream.ToArray();
                var newUserImage = new UserImage
                {
                    UserID = command.UserID,
                    Data = data,
                    Name = file.FileName,
                    ContentType = file.ContentType,

                };
                context.UserImage.Add(newUserImage);
                await context.SaveChangesAsync(cancellationToken);
            }
            return command.UserID;
        }
    }
}
