using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Places.Cities.Queries.Lists
{
    public sealed class ListCitiesQueryHandler(IAppDbContext context) :
        IRequestHandler<ListCitiesQuery,PageResult<ListCitiesQueryDto>>
    {
        public async Task<PageResult<ListCitiesQueryDto>> Handle(ListCitiesQuery request,
            CancellationToken ct)
        {
            var cit = context.Cities.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                cit = cit.Where(x => x.Name.Contains(request.Search));
            }
            var result = cit.OrderBy(x => x.Name).
                Select(x => new ListCitiesQueryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                });
            return await PageResult<ListCitiesQueryDto>.
                FromQueryableAsync(result, request.Paging, ct);
        }
    }
}
