using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Commands.Update
{
    public sealed class UpdateAnimalCommandHandler(IAppDbContext context)
           : IRequestHandler<UpdateAnimalCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
        {
            var animal = await context.Animals.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (animal == null)
                throw new PawPalNotFoundException($"Animal with Id {request.Id} does not exist!");

            var gender = await context.Genders.Where(x => x.GenderName.ToLower() == request.Gender.ToLower()).FirstOrDefaultAsync();
            if (gender == null)
                throw new PawPalNotFoundException($"Gender with the name {request.Gender} is not valid!");

            var category = await context.AnimalCategories.Where(x => x.CategoryName.ToLower() == request.Category.ToLower()).FirstOrDefaultAsync();
            if (category == null)
                throw new PawPalNotFoundException($"Category with the name {request.Category} is not valid!");

            var breedsList = await context.Breeds.Where(b => b.CategoryID == category.Id).ToListAsync(cancellationToken);

            var validatedBreed = "";
            foreach (var breed in breedsList)
            {
                if (breed.Name.ToLower().Equals(request.Breed.ToLower()))
                {
                    validatedBreed = breed.Name;
                    break;
                }
            }
            if (validatedBreed.Equals(""))
                throw new PawPalConflictException($"{request.Breed} is not a breed that belongs to " +
                    $"the category {category.CategoryName}!");

            animal.Name = string.IsNullOrWhiteSpace(request.Name) ? animal.Name : request.Name.Trim();
            animal.Breed = string.IsNullOrWhiteSpace(request.Breed) ? animal.Breed : validatedBreed.Trim();
            animal.Age = request.Age;
            animal.ChildFriendly = request.ChildFriendly;
            animal.HasPapers = request.HasPapers;
            animal.Gender = request.Gender == null ? animal.Gender : gender;
            animal.Category = request.Category == null ? animal.Category : category;

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

    }
}
