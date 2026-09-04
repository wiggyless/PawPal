


namespace PawPal.Application.Modules.News.Queries.List
{
    public sealed class ListNewsQueryHandler(IAppDbContext ctx)
        : IRequestHandler<ListNewsQuery, PageResult<ListNewsQueryDto>>
    {
        public async Task<PageResult<ListNewsQueryDto>> Handle(ListNewsQuery request, CancellationToken cancellationToken)
        {
            var query = ctx.News.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(n =>
                    n.Title.Contains(request.Search) || n.Content.Contains(request.Search));
            }

            if (request.PublishedFrom is not null)
                query = query.Where(n => n.PublishedAt >= request.PublishedFrom);

            if (request.PublishedTo is not null)
                query = query.Where(n => n.PublishedAt <= request.PublishedTo);

            if (request.HasPhoto is not null)
                query = request.HasPhoto == true
                    ? query.Where(n => n.PhotoURL != null && n.PhotoURL != string.Empty)
                    : query.Where(n => n.PhotoURL == null || n.PhotoURL == string.Empty);

            query = request.SortDescending
                ? query.OrderByDescending(n => n.PublishedAt)
                : query.OrderBy(n => n.PublishedAt);

            var projectedQuery = query
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
