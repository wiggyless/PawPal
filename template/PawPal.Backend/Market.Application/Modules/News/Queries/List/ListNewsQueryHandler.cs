


namespace PawPal.Application.Modules.News.Queries.List
{
    public sealed class ListNewsQueryHandler(IAppDbContext ctx)
        : IRequestHandler<ListNewsQuery, PageResult<ListNewsQueryDto>>
    {
        public async Task<PageResult<ListNewsQueryDto>> Handle(ListNewsQuery request, CancellationToken cancellationToken)
        {
            var query = ctx.News.AsNoTracking();

            if(!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(n =>
                    n.Title.Contains(request.Search));
            }

            var projectedQuery = query
                .OrderByDescending(n => n.PublishedAt)
                .Select(n => new ListNewsQueryDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    PublishedAt = n.PublishedAt,
                    PhotoURL = n.PhotoURL
                });

            return await PageResult<ListNewsQueryDto>
                .FromQueryableAsync(projectedQuery, request.Paging, cancellationToken);
        }
    }
}
