using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Places.Cantons.Queries_
{
    public class GetCantonByQueryHandler(IAppDbContext context) : 
        IRequestHandler<GetCantonByIdQuery,GetCantonByIdQueryDto>
    {
        public async Task<GetCantonByIdQueryDto> Handle(GetCantonByIdQuery request,CancellationToken token)
        {
            var cities = context.Cities.AsNoTracking();
            var canton = await context.Cantons.
                Where(c => c.Id == request.Id).
                Select(x => new GetCantonByIdQueryDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Cities = cities.Where(y => y.CantonId == x.Id).Select(
                        z => new CantonCitiesID
                        {
                            Id = z.Id,
                            Name = z.Name,
                        }
                    ).ToList(),
                }).FirstOrDefaultAsync(token);
            if (canton == null) {
                throw new PawPalNotFoundException("Canton not found");
            }
            return canton;
        }
    }
}
