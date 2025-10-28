using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Queries.List
{
    public sealed class ListAnimalCategoryQueryHandler(IAppDbContext context)
        : IRequestHandler<ListAnimalCategoriesQuery, PageResult<ListAnimalCategoriesQueryDto>>
    {
        public async Task<PageResult<ListAnimalCategoriesQueryDto>> Handle(ListAnimalCategoriesQuery request, CancellationToken cancellationToken)
        {
            var q = context.AnimalCategories.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                q = q.Where(x => x.CategoryName.Contains(request.Search));
            }

            var resultQuery = q.OrderBy(x => x.CategoryName)
                .Select(x => new ListAnimalCategoriesQueryDto
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName
                });
            return await PageResult<ListAnimalCategoriesQueryDto>.FromQueryableAsync(resultQuery, request.Paging, cancellationToken);
        }
    }
}
