using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalBreed.Queries.GetById
{
    public sealed class GetAnimalBreedByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetAnimalBreedByIdQuery,GetAnimalBreedByIdQueryDto>
    {
        public async Task<GetAnimalBreedByIdQueryDto> Handle(GetAnimalBreedByIdQuery request,CancellationToken cancellationToken)
        {
            var breed = await context.Breeds.Where(x => x.Id == request.Id).
                Select(x => new GetAnimalBreedByIdQueryDto {
                    Id = request.Id,
                    CategoryId = x.CategoryID,
                    Name = x.Name,
                }).FirstOrDefaultAsync(cancellationToken);
            if (breed is null) throw new PawPalNotFoundException($"Animal breed by with {request.Id} not found");
            return breed;
        }
    }
}
