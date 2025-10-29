using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Queries.GetById
{
    public sealed class GetAnimalByIdQueryHandler(IAppDbContext context)
        : IRequestHandler<GetAnimalByIdQuery, GetAnimalByIdQueryDto>
    {
        public async Task<GetAnimalByIdQueryDto> Handle(GetAnimalByIdQuery request, CancellationToken cancellationToken)
        {
            var animal = await context.Animals.
                Where(a => a.Id == request.Id).
                Select(x => new GetAnimalByIdQueryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Breed = x.Breed,
                    Gender = x.Gender.GenderName,
                    Age = x.Age,
                    HasPapers = x.HasPapers,
                    ChildFriendly = x.ChildFriendly,
                    Category = x.Category.CategoryName
                }).FirstOrDefaultAsync(cancellationToken);

            if (animal == null) throw new PawPalNotFoundException($"Animal with Id {request.Id} does not exist!");

            return animal;
                

        }
    }
}
