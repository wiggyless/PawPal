using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Commands.Create
{
    public class CreateAnimalCommandHandler(IAppDbContext context)
        : IRequestHandler<CreateAnimalCommand, int>
    {
        public async Task<int> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
        {
            var gender = await context.Genders.Where(x => x.Id == request.GenderId).FirstOrDefaultAsync(cancellationToken);
            if (gender == null)
                throw new PawPalNotFoundException($"Gender with Id {request.GenderId} does not exist!");

            var category = await context.AnimalCategories.Where(c => c.Id == request.CategoryId).FirstOrDefaultAsync(cancellationToken);
            if(category == null)
                throw new PawPalNotFoundException($"Category with Id {request.CategoryId} does not exist!");

            var animal = new AnimalEntity
            {
                Name = request.Name,
                Breed = request.Breed,
                Age = request.Age,
                GenderId = request.GenderId,
                Gender = gender,
                HasPapers = request.HasPapers,
                ChildFriendly = request.ChildFriendly,
                CategoryId = request.CategoryId,
                Category = category
            };

            context.Animals.Add(animal);
            await context.SaveChangesAsync(cancellationToken);

            return animal.Id;
        }
    }
}
