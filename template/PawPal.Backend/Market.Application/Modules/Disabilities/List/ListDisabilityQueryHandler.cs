using PawPal.Application.Modules.Allergies.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Disabilities.List
{
    public sealed class ListDisabilityQueryHandler(IAppDbContext context) : IRequestHandler<ListDisabilityQuery, PageResult<ListDisabilityQueryDto>>
    {
        public async Task<PageResult<ListDisabilityQueryDto>> Handle(ListDisabilityQuery request, CancellationToken cancellationToken)
        {
            var disabilityList = context.Disabilities.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchName))
            {
                disabilityList = disabilityList.Where(x => x.Name.ToLower().Contains(request.SearchName.ToLower()));
            }
            var finalList = disabilityList.OrderBy(x => x.Name).Select(x => new ListDisabilityQueryDto
            {
                Id = x.Id,
                Name = x.Name,
            });
            return await PageResult<ListDisabilityQueryDto>.FromQueryableAsync(finalList, request.Paging, cancellationToken);
        }
    }
}
