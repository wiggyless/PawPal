using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Queries.List
{
    public class ListCommentReportsQueryHandler(IAppDbContext context,IAppCurrentUser currentUser)
        : IRequestHandler<ListCommentReportsQuery, PageResult<ListCommentReportsQueryDto>>
    {
        public async Task<PageResult<ListCommentReportsQueryDto>> Handle(ListCommentReportsQuery request, CancellationToken cancellationToken)
        {
            var q = context.ReportedComments.Include(x=>x.Comment).Include(x=>x.Comment.User).AsNoTracking();
            var qIDS = q.Select(x => x.CommentReportedBy);
            var users = context.Users.AsNoTracking().Where(x => qIDS.Contains(x.Id));
            var reportedCommentsIDs = q.Select(x => x.CommentID);
            if(currentUser.RoleId != 3 || !currentUser.IsAuthenticated)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            if (request.DateMin is not null)
            {
                q = q.Where(x => x.DateReported >= request.DateMin);
            }
            if (request.DateMax is not null) {
                q = q.Where(x =>  x.DateReported <= request.DateMax);
            }
            var res = q.GroupJoin(
                users,
                repCom => repCom.CommentReportedBy,
                usr => usr.Id,
                (commentReport, usr) => new { commentReport, usr })
             .SelectMany(
            x => x.usr.DefaultIfEmpty(),
            (x, usr) => new ListCommentReportsQueryDto
            {
                Id = x.commentReport.Id,
                Comment = new ListReportedCommentEntityDto
                {
                    Id = x.commentReport.Comment.Id,
                    Content = x.commentReport.Comment.Content,
                    DatePosted = x.commentReport.Comment.DatePosted,
                    PostId = x.commentReport.Comment.PostId,
                    UserId = x.commentReport.Comment.UserId,
                },
                Username = usr.Username,
                DateReported = x.commentReport.DateReported,
                CommentReportedByID = x.commentReport.CommentReportedBy,
                Description = x.commentReport.Description,
                Reason = x.commentReport.Reason,
            }
            ).OrderBy(x=>x.DateReported);
            return await PageResult<ListCommentReportsQueryDto>.FromQueryableAsync(res, request.Paging, cancellationToken);
        }
    }
}
