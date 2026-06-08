using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UserImages.Commands.Update
{
    public class UpdateUserImageCommandHandler(IAppDbContext context) : IRequestHandler<UpdateUserImageCommand,int>
    {
        public async Task<int> Handle(UpdateUserImageCommand command,CancellationToken cancellationToken)
        {
            var user = await context.Users.Where(x => x.Id == command.UserID).AsNoTracking().FirstOrDefaultAsync();
            var userImage = await context.UserImage.Where(x => x.UserID == command.UserID).FirstOrDefaultAsync();
            if (user is null)
            {
                throw new PawPalNotFoundException("User does not exist inside the database");
            }
            if (userImage is null) {
                throw new PawPalNotFoundException("User image does not exist inside the database");
            }
            var file = command.Image;
            if (file is null || file.Length == 0)
            {
                throw new PawPalNotFoundException("Image has not been sent properly");
            }
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] data = memoryStream.ToArray();
                userImage.Data = data;
                userImage.Name = file.FileName;
                userImage.ContentType = file.ContentType;
                await context.SaveChangesAsync(cancellationToken);
            }
            return command.UserID;
        }
    }
}
