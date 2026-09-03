using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.LikedUserPosts.Commands.Delete
{
    public sealed class DeleteLikeUserPostCommandHandler(IAppCurrentUser user,IAppDbContext context) : IRequestHandler<DeleteLikedUserPostCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteLikedUserPostCommand command,CancellationToken cancellationToken)
        {
            if (user.UserId is null)
            {
                throw new PawPalConflictException("User is not allowed to do this");
            }
            var userId = user.UserId.Value;

            var likedUserList = await context.LikedUserPosts
                .FirstOrDefaultAsync(x => x.UserId == userId && command.PostId == x.PostId, cancellationToken);
            if(likedUserList is null)
            {
                throw new PawPalNotFoundException("Does not exist");
            }
            context.LikedUserPosts.Remove(likedUserList);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
