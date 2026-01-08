using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Commands.Delete
{
    public sealed class DeletePostCommandHandler(IAppDbContext context,IAppCurrentUser currentUser) : IRequestHandler<DeletePostCommand,Unit>
    {
        public async Task<Unit> Handle(DeletePostCommand request,CancellationToken cancellationToken)
        {
            if (currentUser is null) throw new MarketBusinessRuleException("123", "User isn't authorized to do this");
            var post = await context.Posts.Include(x=>x.Animal).FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
            var postImage = await context.PostImages.FirstOrDefaultAsync(x => x.PostId == request.Id, cancellationToken);
            if (post== null) throw new PawPalNotFoundException($"Post with Id {request.Id} does not exist!");
            post.IsDeleted = true;
            post.Animal.IsDeleted = true;
            if (postImage != null) postImage.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
