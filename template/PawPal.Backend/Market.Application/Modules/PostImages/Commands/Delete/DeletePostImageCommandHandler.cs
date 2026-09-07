using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Shared.Constants;

namespace PawPal.Application.Modules.PostImages.Commands.Delete
{
    public class DeletePostImageCommandHandler(IAppCurrentUser user,IAppDbContext context, IFileStorageService fileStorage)
        : IRequestHandler<DeletePostImageCommand,Unit>
    {
        public async Task<Unit> Handle(DeletePostImageCommand command,CancellationToken cancellationToken)
        {
            var post = await context.Posts.Where(x => x.Id == command.PostId).FirstOrDefaultAsync(cancellationToken);
            // Fail closed: a missing post (e.g. already soft-deleted) must not skip the
            // ownership check — it previously let anyone delete that post's images/folder.
            if (post is null)
                throw new PawPalNotFoundException($"Post with id {command.PostId} not found");
            if (post.UserId != user.UserId && user.RoleId != Roles.Admin)
                throw new MarketBusinessRuleException("123", "User isn't authorized to do this");
            var postImage = await context.PostImages.Where(x => x.PostId == command.PostId).FirstOrDefaultAsync(cancellationToken);
            fileStorage.DeleteFolder($"posts/Post_{command.PostId}");
            if (postImage is null) return Unit.Value;
            postImage.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
