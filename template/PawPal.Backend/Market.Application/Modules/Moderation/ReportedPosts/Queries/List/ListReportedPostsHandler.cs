
namespace PawPal.Application.Modules.Moderation.ReportedPosts.Queries.List
{
    public class ListReportedPostsHandler(IAppDbContext context)
        : IRequestHandler<ListReportedPostsQuery, PageResult<ListReportedPostsQueryDto>>
    {
        public async Task<PageResult<ListReportedPostsQueryDto>> Handle(ListReportedPostsQuery request, CancellationToken cancellationToken)
        {
            var q = context.ReportedPosts.AsNoTracking();
            var projectedQuery = q.OrderBy(x => x.Reason)
            .Select(x => new ListReportedPostsQueryDto
            {
                Reason = x.Reason,
                Description = x.Description,
                PostID = x.PostID,
                UserID=  x.UserID,
            });

            return await PageResult<ListReportedPostsQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, cancellationToken);
        }
    }
}
