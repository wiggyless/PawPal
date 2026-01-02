using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Domain.Entities.Animal_Info;
namespace PawPal.Application.Modules.Animal_Info.AnimalBreed.Commands.Create
{
    internal class CreateAnimalBreedHandler(IAppDbContext context) : IRequestHandler<CreateAnimalBreedCommand,int>
    {
        public async Task<int> Handle(CreateAnimalBreedCommand request,CancellationToken cancellationToken)
        {
            if (request.Name is null)
                throw new PawPalConflictException("Name cannot be null");
            var category = await context.AnimalCategories.Where(x => x.Id == request.CategoryId).FirstOrDefaultAsync(cancellationToken);
            if (category is null)
                throw new PawPalNotFoundException("Category does not exist");
            var newBreed = new BreedEntity
            {
                Name = request.Name,
                CategoryID = category.Id,
            };
            context.Breeds.Add(newBreed);
            await context.SaveChangesAsync(cancellationToken);
            return newBreed.Id;
        }
    }
}
