using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.Commands.Update
{
    public class UpdatePostImageCommandHandle(IAppDbContext context) : IRequestHandler<UpdatePostImageCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePostImageCommand command,CancellationToken cancellationToken)
        {
            var postImage = await context.PostImages.Where(x=>x.PostId == command.PostId).FirstOrDefaultAsync(cancellationToken);
            if (postImage is null)
            { 
                throw new PawPalNotFoundException($"Post with id {command.PostId} not found");
            }
            if(postImage.PhotoURL is null)
            {
                postImage.PhotoURL = new List<string>(command.PostImages);
            }else
            {
                postImage.PhotoURL.AddRange(command.PostImages);
            }
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
