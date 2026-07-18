using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PawPal.Application.Modules.Moderation.ReportedPosts.Queries.List
{
    public class ListReportedPostsHandler(IAppDbContext context)
        : IRequestHandler<ListReportedPostsQuery, PageResult<ListReportedPostsQueryDto>>
    {
        public async Task<PageResult<ListReportedPostsQueryDto>> Handle(ListReportedPostsQuery request, CancellationToken cancellationToken)
        {
            var query = context.ReportedPosts
                .AsNoTracking()
                .Where(rp => context.Posts.Any(p => p.Id == rp.PostID))
                .Select(rp => new ListReportedPostsQueryDto
                {
                    Id = rp.Id,
                    UserID = rp.UserID,
                    PostID = rp.PostID,
                    Description = rp.Description,
                    Reason = rp.Reason,

                    Username = context.Users.Where(u => u.Id == rp.UserID).Select(u => u.Username).FirstOrDefault() ?? "",
                    DateSent = rp.DateSent
                })
                .OrderBy(x => x.DateSent);
            return await PageResult<ListReportedPostsQueryDto>.FromQueryableAsync(query, request.Paging, cancellationToken);
        }
    }
}