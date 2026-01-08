using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.Commands.Create
{
    public class CreatePostImageCommandHandler(IAppCurrentUser user,IAppDbContext context) : 
        IRequestHandler<CreatePostImageCommand,int>
    { 
        public async Task<int> Handle(CreatePostImageCommand command,CancellationToken cancellationToken)
        {
            var post = await context.Posts.Where(x=>x.Id == command.PostId).FirstOrDefaultAsync(cancellationToken);
            if (post is null)
                throw new PawPalNotFoundException($"Post with ID:{command.PostId} not found");
            var newPostImages = new PostImagesEntity
            {
                PostId = command.PostId,
                PhotoURL = command.PostImages,
            };
            context.PostImages.Add(newPostImages);
            await context.SaveChangesAsync(cancellationToken);
            return newPostImages.Id;
        }
    }
}
