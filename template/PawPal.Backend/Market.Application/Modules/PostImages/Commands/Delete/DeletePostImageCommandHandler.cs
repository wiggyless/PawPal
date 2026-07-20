using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.Commands.Delete
{
    public class DeletePostImageCommandHandler(IAppCurrentUser user,IAppDbContext context)
        : IRequestHandler<DeletePostImageCommand,Unit>
    {
        public async Task<Unit> Handle(DeletePostImageCommand command,CancellationToken cancellationToken)
        {
            var post = await context.Posts.Where(x => x.Id == command.PostId).FirstOrDefaultAsync(cancellationToken);
            if (post is not null && post.UserId != user.UserId && user.RoleId != 3)
                throw new MarketBusinessRuleException("123", "User isn't authorized to do this");
            var postImage = await context.PostImages.Where(x => x.PostId == command.PostId).FirstOrDefaultAsync(cancellationToken);
            if (postImage is null) return Unit.Value;
            postImage.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
