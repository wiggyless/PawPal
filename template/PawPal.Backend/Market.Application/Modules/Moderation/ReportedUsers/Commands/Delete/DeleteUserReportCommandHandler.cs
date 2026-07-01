using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Delete
{
    public class DeleteUserReportCommandHandler(IAppDbContext context,IAppCurrentUser currentUser):
        IRequestHandler<DeleteUserReportCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteUserReportCommand request, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != 3 || !currentUser.IsAuthenticated)
                throw new PawPalConflictException("You're not authorized for this action!");

            var reports = await context.ReportedUsers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (reports is null)
                throw new PawPalConflictException("Report does not exist!");

            reports.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
