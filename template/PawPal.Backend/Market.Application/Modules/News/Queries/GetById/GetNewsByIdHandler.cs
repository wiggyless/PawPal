using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.News.Queries.GetById
{
    public class GetNewsByIdHandler(IAppDbContext context) : 
        IRequestHandler<GetNewsByIdQuery, GetNewsByIdQueryDto>
    {
        public async Task<GetNewsByIdQueryDto> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var news = await context.News
                .Where(n => n.Id == request.Id)
                .Select(x => new GetNewsByIdQueryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    PublishedAt = x.PublishedAt,
                    PhotoURL = x.PhotoURL
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (news == null)
            {
                throw new PawPalNotFoundException($"News item with Id {request.Id} not found.");
            }
            return news;
        }
    }
}
