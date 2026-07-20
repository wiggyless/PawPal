using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalBreed.Commands.Delete
{
    public sealed class DeleteAnimalBreedHandler(IAppDbContext context,IAppCurrentUser user) 
        : IRequestHandler<DeleteAnimalBreedCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteAnimalBreedCommand request,CancellationToken cancellationToken)
        {
            if (user.RoleId != 3)
                throw new PawPalConflictException("Only administrators can delete animal breeds.");

            var breed = await context.Breeds.Where(x=>x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (breed is null)
                throw new PawPalNotFoundException("Breed does not exist");
            breed.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
