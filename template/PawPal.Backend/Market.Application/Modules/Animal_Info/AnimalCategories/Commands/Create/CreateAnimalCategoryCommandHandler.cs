﻿using System;
﻿using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Create
{
    public sealed class CreateAnimalCategoryCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<CreateAnimalCategoryCommand, int>
    {
        public async Task<int> Handle(CreateAnimalCategoryCommand request, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != 3)
                throw new PawPalConflictException("Only administrators can create animal categories.");

            var categoryName = request.CategoryName?.Trim();

            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ValidationException("Name is required!");

            //checking if the same category exists, name wise
            bool categoryExists = await context.AnimalCategories.
                AnyAsync(x => x.CategoryName == categoryName, cancellationToken);

            if (categoryExists)
                throw new PawPalConflictException("Category with the same name already exists!");

            var animalCategory = new AnimalCategoriesEntity
            {
                CategoryName = request.CategoryName!.Trim()
            };
            context.AnimalCategories.Add(animalCategory);
            await context.SaveChangesAsync(cancellationToken);
            return animalCategory.Id;
        }
    }
}
