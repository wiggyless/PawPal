using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Gender.List
{
    public sealed class ListGenderQueryHandler(IAppDbContext context) : IRequestHandler<ListGenderQuery, PageResult<ListGenderQueryDto>>
    {
        public async Task<PageResult<ListGenderQueryDto>> Handle(ListGenderQuery request,CancellationToken cancellationToken)
        {
            var genders = context.Genders.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchName))
                genders = genders.Where(x => x.GenderName.ToLower().Contains(request.SearchName));
            if (genders is null)
                throw new PawPalNotFoundException("Gender does not exist");
            var finalList = genders.OrderBy(x => x.GenderName).Select(x => new ListGenderQueryDto
            {
                Id = x.Id,
                Name = x.GenderName,
            });
            return await PageResult<ListGenderQueryDto>.FromQueryableAsync(finalList,request.Paging,cancellationToken);
        }
    }
}
