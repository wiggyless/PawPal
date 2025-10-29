using PawPal.Application.Modules.Catalog.ProductCategories.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Update
{
    public sealed class UpdateAnimalCategoryCommandHandler(IAppDbContext context)
            : IRequestHandler<UpdateAnimalCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAnimalCategoryCommand request, CancellationToken cancellationToken)
        {
            var animalCategory = await context.AnimalCategories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (animalCategory == null)
                throw new PawPalNotFoundException($"Animal category with Id {request.Id} does not exist!");

            var categoryExists = await context.AnimalCategories.AnyAsync(x =>
            x.CategoryName.ToLower() == request.CategoryName.ToLower()
            && x.Id != request.Id, cancellationToken);

            if (categoryExists)
                throw new PawPalConflictException("This category already exists!");

            animalCategory.CategoryName = request.CategoryName.Trim();
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
