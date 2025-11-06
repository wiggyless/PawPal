using PawPal.Application.Modules.Places.Cities.Queries.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Places.Cantons.Lists
{
    public sealed class ListCantonsQueryHandler(IAppDbContext db):
        IRequestHandler<ListCantonsQuery,PageResult<ListCantonsQueryDto>>
    {
        public async Task<PageResult<ListCantonsQueryDto>> Handle(ListCantonsQuery 
            request,CancellationToken ct)
        {
            var canton = db.Cantons.AsNoTracking();
            var cities = db.Cities.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                canton = canton.Where(x => x.FullName.Contains(request.Search));
            }
            
            var result = canton.OrderBy(x => x.FullName).
                Select(x => new ListCantonsQueryDto
                {
                    Id = x.Id,
                    FullName= x.FullName,
                    Cities = cities.Where(y => y.CantonId == x.Id).Select(
                        z => new ListCantonsQueryCitiesDto
                        {
                            Id = z.Id,
                            Name = z.Name,
                        }
                    ).ToList(),
                });

            return await PageResult<ListCantonsQueryDto>.FromQueryableAsync
                (result, request.Paging, ct);
        }
    }
}
