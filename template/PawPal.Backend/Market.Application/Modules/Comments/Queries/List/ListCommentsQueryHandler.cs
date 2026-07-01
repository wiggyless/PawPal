using PawPal.Application.Modules.Gender.List;
using PawPal.Application.Modules.Moderation.ReportedPosts.Queries.List;
using PawPal.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Comments.Queries.List
{
    public sealed class ListCommentsQueryHandler(IAppDbContext context) : 
        IRequestHandler<ListCommentsQuery,PageResult<ListCommentsQueryDto>>
    {
        public async Task<PageResult<ListCommentsQueryDto>> Handle(ListCommentsQuery request,CancellationToken cancellationToken)
        {
            var post = context.Posts.Where(x => x.Id == request.PostID).AsNoTracking().FirstOrDefault();
            if(post is null)
            {
                throw new PawPalNotFoundException("Post does not exist");
            }
            var userImages = context.UserImage.AsNoTracking();
            var comments = context.Comments.Include(x=>x.User).Where(x => x.PostId == post.Id && !x.User.isUserDisabled).AsQueryable();
            var result = comments
          .GroupJoin(
          userImages,
          comm => comm.UserId,
          ui => ui.UserID,
          (comments, userImage) => new { comments, userImage })
           .SelectMany(
          x => x.userImage.DefaultIfEmpty(),
          (x, usrImg) => new ListCommentsQueryDto
          {
              CommentID = x.comments.Id,
              Content = x.comments.Content,
              UserID = x.comments.UserId,
              Username = x.comments.User.Username,
              DatePosted = x.comments.DatePosted,
              PhotoURL = usrImg.PhotoURL ?? "",

          }).OrderByDescending(x => x.DatePosted);
            return await PageResult<ListCommentsQueryDto>.FromQueryableAsync(result, request.Paging, cancellationToken);
        }
    }
}
