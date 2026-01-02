using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalBreed.Queries.List
{
    public sealed class ListAnimalBreedQueryHandler(IAppDbContext context) : IRequestHandler<ListAnimalBreedQuery,PageResult<ListAnimalBreedQueryDto>>
    {
        public async Task<PageResult<ListAnimalBreedQueryDto>> Handle(ListAnimalBreedQuery request,CancellationToken cancellationToken)
        {
            var breedList = context.Breeds.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchName))
                breedList = breedList.Where(x => x.Name.ToLower().Contains(request.SearchName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchCategoryName))
            {
                var category = await context.AnimalCategories.Where(x => x.CategoryName == request.SearchCategoryName).FirstOrDefaultAsync(cancellationToken);
                if(category is not null)
                    breedList = breedList.Where(x => x.CategoryID == category.Id);
            }
            var finalList = breedList.OrderBy(x=>x.Name).Select(x => new ListAnimalBreedQueryDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryID,
            });
            return await PageResult<ListAnimalBreedQueryDto>.FromQueryableAsync(finalList,request.Paging,cancellationToken);

        }
    }
}
