using PawPal.API.Controllers.Moderation.ReportedPosts.Commands.Create;
using PawPal.Application.Modules.Moderation.ReportedProblems.Commands.Delete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedPosts.Commands.Delete
{
    public class DeleteReportedPostHandler(IAppDbContext context, IAppCurrentUser currentUser) :
        IRequestHandler<DeleteProblemReportCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteProblemReportCommand request, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != 3 || !currentUser.IsAuthenticated)
                throw new PawPalConflictException("You're not authorized for this actioN!");

            var reportedPost = await context.ReportedPosts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (reportedPost is null)
                throw new PawPalConflictException("Post does not exist!");

            reportedPost.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
