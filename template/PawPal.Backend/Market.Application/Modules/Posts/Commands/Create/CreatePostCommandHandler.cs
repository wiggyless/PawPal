using PawPal.Application.Modules.Users.Commands.Create;
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Domain.Entities.Animal_Info;
namespace PawPal.Application.Modules.Posts.Commands.Create
{
    public sealed class CreatePostCommandHandler(IAppDbContext context,IAppCurrentUser currentUser)
            : IRequestHandler<CreatePostCommand, int>
    {
        public async Task<int> Handle(CreatePostCommand request,CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != 2 || !currentUser.IsAuthenticated)
            {
                throw new PawPalConflictException("User is not verified to make this action");
            }
            var user = await context.Users.Where(x => x.Id == currentUser.UserId).FirstOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                throw new PawPalNotFoundException("User does not exist inside the database");
            }
            var animal = await context.Animals.Where(x => x.Id == request.AnimalID).FirstOrDefaultAsync(cancellationToken);
            if (animal == null)
            {
                throw new PawPalNotFoundException("Animal does not exist inside the database");
            }
            var health = await context.AnimalHealthHistories.Where(x => x.AnimalId == animal.Id).FirstOrDefaultAsync(cancellationToken);


            var newPost = new PostsEntity
            {
                UserId = user.Id,
                AnimalID = animal.Id,
                AnimalHealthHistory = health,
                DateAdded = DateTime.Now,
                CityId = user.CityId,
                Status = "active",
            };
            context.Posts.Add(newPost);
            await context.SaveChangesAsync(cancellationToken);
            return newPost.Id;
        }
    }
}
