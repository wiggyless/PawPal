using PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Delete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Commands.Delete
{
    public sealed class DeleteCommentReportCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) :
        IRequestHandler<DeleteCommentReportCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCommentReportCommand request, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != 3)
                throw new PawPalConflictException("You're not authorized for this action!");
            if (!currentUser.IsAuthenticated)
            {
                throw new PawPalConflictException("You're not authorized for this action!");
            }
            var reportedComment = await context.ReportedComments.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (reportedComment is null)
                throw new PawPalConflictException("Report does not exist!");

            reportedComment.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
