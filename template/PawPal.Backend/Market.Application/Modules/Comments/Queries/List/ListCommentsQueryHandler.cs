using PawPal.Application.Modules.Gender.List;
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
            var comments = context.Comments.Include(x=>x.User).Where(x => x.PostId == post.Id).AsQueryable();
            var finalList = comments.OrderBy(x => x.DatePosted).Select(x => new ListCommentsQueryDto
            {
                CommentID = x.Id,
                Content = x.Content,
                UserID = x.UserId,
                Username = x.User.Username,
                DatePosted = x.DatePosted,
            });
            return await PageResult<ListCommentsQueryDto>.FromQueryableAsync(finalList, request.Paging, cancellationToken);
        }
    }
}
