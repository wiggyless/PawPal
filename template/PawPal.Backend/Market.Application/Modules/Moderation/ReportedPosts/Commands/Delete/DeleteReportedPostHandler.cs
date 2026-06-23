using PawPal.API.Controllers.Moderation.ReportedPosts.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedPosts.Commands.Delete
{
    public class DeleteReportedPostHandler(IAppDbContext context, IAppCurrentUser currentUser) :
        IRequestHandler<DeleteReportedPostCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteReportedPostCommand request, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != 3)
                throw new PawPalConflictException("You're not authorized for this actioN!");

            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == request.PostID, cancellationToken);
            if (post is null)
                throw new PawPalConflictException("Post does not exist!");

            post.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
