using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.Commands.Update
{
    public class UpdatePostImageCommandHandler(IAppDbContext context, IAppCurrentUser user) : IRequestHandler<UpdatePostImageCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePostImageCommand command,CancellationToken cancellationToken)
        {
            var postImage = await context.PostImages.Where(x=>x.PostId == command.PostId).FirstOrDefaultAsync(cancellationToken);
            if (postImage is null)
            {
                throw new PawPalNotFoundException($"Post with id {command.PostId} not found");
            }
            var post = await context.Posts.Where(x => x.Id == command.PostId).FirstOrDefaultAsync(cancellationToken);
            if (post is not null && post.UserId != user.UserId && user.RoleId != 3)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            if (command.PostImages is null || command.PostImages.Count == 0)
            {
                throw new ValidationException("At least one image is required.");
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
