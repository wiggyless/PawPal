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
    public sealed class CreatePostCommandHandler(IAppDbContext context)
            : IRequestHandler<CreatePostCommand, int>
    {
        public async Task<int> Handle(CreatePostCommand request,CancellationToken cancellationToken)
        {
            var user = await context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);
            var animal = await context.Animals.Where(x => x.Id == request.AnimalID).FirstOrDefaultAsync(cancellationToken);
            var health = await context.AnimalHealthHistories.Where(x => x.AnimalId == animal.Id).FirstOrDefaultAsync(cancellationToken);
            if(user.RoleId!=2) // probably have to change this shit 
            {
                throw new Exception("User is not verified to make this action");
            }
            if(animal == null)
            {
                throw new PawPalNotFoundException("Animal does not exist inside the database");
            }
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
