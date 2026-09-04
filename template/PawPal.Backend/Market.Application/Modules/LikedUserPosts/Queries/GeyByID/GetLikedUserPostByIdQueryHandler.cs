using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Shared.Constants;

namespace PawPal.Application.Modules.LikedUserPosts.Queries.GeyByID
{
    public sealed class GetLikedUserPostByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<GetLikedUserPostByIdQuery,GetLikedUserPostByQueryDto>
    {
        public async Task<GetLikedUserPostByQueryDto> Handle(GetLikedUserPostByIdQuery request,CancellationToken cancellationToken)
        {
            // A user's liked-posts are private; only that user (or an admin) may view them.
            if (request.UserId != currentUser.UserId && currentUser.RoleId != Roles.Admin)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }

            var likeUserPost = await context.LikedUserPosts.Where(x => x.UserId == request.UserId && x.PostId == request.PostId)
                .Select(x => new GetLikedUserPostByQueryDto
                {
                    PostId = request.PostId,
                    UserId = request.UserId,
                    DateLiked =x.DateLiked,
                }).FirstOrDefaultAsync(cancellationToken);
            if(likeUserPost is null)
            {
                throw new PawPalNotFoundException("Liked user post with ID: " +  request.PostId + "and User ID:"+ request.UserId + " does not exist");
            }
            return likeUserPost;
        }

    }
}
