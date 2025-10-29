using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Delete
{
    public class DeleteAnimalCategoryCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser) :
        IRequestHandler<DeleteAnimalCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAnimalCategoryCommand request, CancellationToken cancellationToken)
        {
            if (appCurrentUser.UserId is null)
                throw new MarketBusinessRuleException("123", "User isn't authorized to do this."); //this will change later

            var animalCategory = await context.AnimalCategories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (animalCategory == null)
                throw new PawPalNotFoundException("This category does not exist.");

            animalCategory.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
