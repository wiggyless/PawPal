using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Queries.GetById
{
    public class GetCommentReportByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<GetCommentReportByIdQuery, GetCommentReportByIdQueryDto>
    {
        public async Task<GetCommentReportByIdQueryDto> Handle(GetCommentReportByIdQuery request, CancellationToken cancellationToken)
        {
            var q = await context.ReportedComments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (q is null)
            {
                throw new PawPalConflictException("User report does not exist");
            }
            if (currentUser.UserId != q.CommentReportedBy && currentUser.RoleId != 3)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var user = context.Users.AsNoTracking().FirstOrDefault(x => x.Id == q.CommentReportedBy);
            var comment = context.Comments.AsNoTracking().Include(x=>x.User).FirstOrDefault(x => x.Id == q.CommentID);
            if(user is null)
            {
                throw new PawPalNotFoundException("User does not exist");
            }
            if (comment is null)
            {
                throw new PawPalNotFoundException("Comment does not exist");
            }
            var commentDto = new ReportedCommentEntityDto
            {
                Id = comment.Id,
                Content = comment.Content,
                DatePosted = comment.DatePosted,
                UserId = comment.UserId,
                PostId = comment.PostId,
                PhotoURL = comment.User.ProfilePictureURL ?? "",
            };
            var projectedQuery = new GetCommentReportByIdQueryDto
            {
                Id = request.Id,
                Reason = q.Reason,
                Description = q.Description,
                CommentReportedByID = q.CommentReportedBy,
                DateReported = q.DateReported,
                Username = user.Username,
                Comment = commentDto,
            };
            return projectedQuery;
        }
    }


}
