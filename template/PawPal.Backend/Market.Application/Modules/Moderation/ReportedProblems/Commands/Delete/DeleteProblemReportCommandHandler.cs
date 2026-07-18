using PawPal.Application.Modules.Moderation.ReportedPosts.Commands.Delete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedProblems.Commands.Delete
{
    public class DeleteProblemReportCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) :
        IRequestHandler<DeleteProblemReportCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteProblemReportCommand request, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != 3 || !currentUser.IsAuthenticated)
                throw new PawPalConflictException("You're not authorized for this actioN!");

            var reportedProblem = await context.ReportProblems.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (reportedProblem is null)
                throw new PawPalConflictException("Reported problem does not exist!");

            reportedProblem.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
