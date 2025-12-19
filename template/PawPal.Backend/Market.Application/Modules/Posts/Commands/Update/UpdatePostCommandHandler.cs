using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Commands.Update
{
    public sealed class UpdatePostCommandHandler(IAppDbContext context)
        : IRequestHandler<UpdatePostCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            // have to fix this - post.Animal.CategoryID big problemus 
            var breedCategory = await context.Breeds.FirstOrDefaultAsync(x => x.Name == request.Breed, cancellationToken);
            
            if (breedCategory == null) throw new PawPalNotFoundException("Breed not found");
            
            if (breedCategory.CategoryID != request.CategoryId)
                throw new PawPalConflictException("Category and breed do not match");

            var post = await context.Posts.Include(x => x.Animal).Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (post == null)
                throw new PawPalNotFoundException("Post entity not found");

            post.PhotoURL = request.PhotoURL;
            post.Animal.CategoryId = request.CategoryId;
            post.Animal.Breed = request.Breed;

            await context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}
