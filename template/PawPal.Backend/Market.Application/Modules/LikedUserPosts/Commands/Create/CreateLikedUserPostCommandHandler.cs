
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PawPal.Application.Modules.LikedUserPosts.Commands.Create
{
    public sealed class CreateLikedUserPostCommandHandler(IAppDbContext context,IAppCurrentUser currentUser) :
        IRequestHandler<CreateLikedUserPostCommand,int>
    {

        public async Task<int> Handle(CreateLikedUserPostCommand command,CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var userId = currentUser.UserId.Value;

            var user = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var likedPosts = context.LikedUserPosts.AsNoTracking();
            if (user is null)
            {
                throw new PawPalNotFoundException("User does not exist");
            }
            if (likedPosts.FirstOrDefault(x => x.PostId == command.PostID && x.UserId == userId) != null){
                return 0;
            }
            var newLikedPost = new Domain.Entities.Posts.LikedUserPosts { PostId = command.PostID, UserId = userId };
            context.LikedUserPosts.Add(newLikedPost);
            await context.SaveChangesAsync(cancellationToken);
            return newLikedPost.Id;
        }
    }
}
