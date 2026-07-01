using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedPosts.Queries.GetById
{
    public sealed class GetReportedPostByIdQueryHandler(IAppDbContext context,IAppCurrentUser currentUser) : IRequestHandler<GetReportedPostByIdQuery, GetReportedPostByIdQueryDto>
    {
        public async Task<GetReportedPostByIdQueryDto> Handle(GetReportedPostByIdQuery request, CancellationToken cancellationToken)
        {
            var q = await context.ReportedPosts.FirstOrDefaultAsync(x => request.Id == request.Id, cancellationToken);
            if (q is null)
            {
                throw new PawPalConflictException("Post report does not exist");
            }
            var post = context.Posts.AsNoTracking().FirstOrDefault(x => x.Id == q.PostID);
            var user = context.Users.AsNoTracking().FirstOrDefault(x => x.Id == q.UserID);
            if(currentUser.UserId != q.UserID && currentUser.RoleId !=3)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            if(post is null)
            {
                throw new PawPalNotFoundException("Post does not exist");
            }

            var projectedQuery = new GetReportedPostByIdQueryDto
            {
                Id = request.Id,
                Reason = q.Reason,
                PostID = q.PostID,
                UserID = q.UserID,
                Description = q.Description,
                DateSent = q.DateSent,
                Username = user.Username ?? "",
            };
            return projectedQuery;
        }
    }
}
