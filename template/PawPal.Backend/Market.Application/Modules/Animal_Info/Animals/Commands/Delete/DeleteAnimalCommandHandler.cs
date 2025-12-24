using PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Delete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Commands.Delete
{
    public class DeleteAnimalCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser) :
        IRequestHandler<DeleteAnimalCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
        {
            var animal = await context.Animals.FirstOrDefaultAsync(x=> x.Id == request.Id, cancellationToken);
            if (animal == null)
                throw new PawPalNotFoundException($"Animal with Id {request.Id} does not exist!");

            animal.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
