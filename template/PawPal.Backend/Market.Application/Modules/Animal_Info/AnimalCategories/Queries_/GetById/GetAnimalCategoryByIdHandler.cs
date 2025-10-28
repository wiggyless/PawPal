using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.GetById
{
    public class GetAnimalCategoryByIdHandler(IAppDbContext context) :
        IRequestHandler<GetAnimalCategoryByIdQuery, GetAnimalCategoryByIdQueryDto>
    {
        public async Task<GetAnimalCategoryByIdQueryDto> Handle(GetAnimalCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await context.AnimalCategories.
                Where(c => c.Id == request.Id)
                .Select(x => new GetAnimalCategoryByIdQueryDto
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (category==null)
                throw new PawPalNotFoundException($"Animal category with Id {request.Id} does not exist!");
            return category;
        }
    }
}
