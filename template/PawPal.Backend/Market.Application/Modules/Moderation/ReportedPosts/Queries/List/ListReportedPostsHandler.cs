
using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.List;
using System.Linq;

namespace PawPal.Application.Modules.Moderation.ReportedPosts.Queries.List
{
    public class ListReportedPostsHandler(IAppDbContext context)
        : IRequestHandler<ListReportedPostsQuery, PageResult<ListReportedPostsQueryDto>>
    {
        public async Task<PageResult<ListReportedPostsQueryDto>> Handle(ListReportedPostsQuery request, CancellationToken cancellationToken)
        {
            var q = context.ReportedPosts.AsNoTracking();
            var posts = context.Posts.AsNoTracking().Select(x=>x.Id).ToList();
            var reportedIDS = q.Where(x => !posts.Contains(x.PostID));
            foreach(var rep in reportedIDS)
            {
                rep.IsDeleted = true;
            }
            await context.SaveChangesAsync(cancellationToken);
            var user = context.Users.AsNoTracking();
            var result = q
           .GroupJoin(
           user,
           uR => uR.UserID,
           u => u.Id,
           (postReport, usr) => new { postReport, usr })
            .SelectMany(
           x => x.usr.DefaultIfEmpty(),
           (x, usr) => new ListReportedPostsQueryDto
           {
               Id = x.postReport.Id,
               UserID = x.postReport.UserID,
               PostID = x.postReport.PostID,
               Description = x.postReport.Description,
               Reason = x.postReport.Reason,
               Username= usr.Username ?? "",
               DateSent = x.postReport.DateSent
         
           }).OrderBy(x => x.DateSent);
            return await PageResult<ListReportedPostsQueryDto>.FromQueryableAsync(result, request.Paging, cancellationToken);
        }
    }
}
