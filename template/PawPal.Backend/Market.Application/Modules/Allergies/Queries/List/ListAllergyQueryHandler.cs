using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Allergies.Queries.List
{
    public sealed class ListAllergyQueryHandler(IAppDbContext context) : IRequestHandler<ListAllergyQuery, PageResult<ListAllergyQueryDto>>
    {
        public async Task<PageResult<ListAllergyQueryDto>> Handle(ListAllergyQuery request,CancellationToken cancellationToken)
        {
            var allergyList = context.Allergies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchName))
            {
                allergyList = allergyList.Where(x=>x.Name.ToLower().Contains(request.SearchName.ToLower()));
            }
            var finalList = allergyList.OrderBy(x => x.Name).Select(x => new ListAllergyQueryDto
            {
                Id = x.Id,
                Name = x.Name,
            });
            return await PageResult<ListAllergyQueryDto>.FromQueryableAsync(finalList,request.Paging,cancellationToken);
        }
    }
}
