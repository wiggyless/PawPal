using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Commands.Update
{
    public sealed class UpdatePostCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<UpdatePostCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {

            var post = await context.Posts.Include(x => x.Animal).Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (post == null)
                throw new PawPalNotFoundException("Post entity not found");
            if (post.UserId != currentUser.UserId && currentUser.RoleId != 3)
                throw new PawPalConflictException("User is not allowed to do this action");

            if (request.Status is not null)
                post.Status = request.Status;
            if (request.CityId is not null)
                post.CityId = request.CityId;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
