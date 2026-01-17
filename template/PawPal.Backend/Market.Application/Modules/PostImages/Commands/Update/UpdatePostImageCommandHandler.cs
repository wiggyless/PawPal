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
            postImage.PhotoURL.Clear();
            var listName = command.PostImages.Select(x => "/posts/Post_" + command.PostId + "/" + x.FileName).ToList();
            for(int i =0;i< listName.Count; i++)
            {
                if (postImage.PhotoURL.Count == 10) break;
                postImage.PhotoURL.Add(listName[i]);
            }
             postImage.MainImage = listName[0];
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
