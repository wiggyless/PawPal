using PawPal.API.Controllers.Moderation.ReportedPosts.Commands.Create;
using PawPal.Domain.Entities.Moderation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedProblems.Commands.Create
{
    public class CreateProblemReportCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) :
        IRequestHandler<CreateProblemReportCommand, int>
    {
        public async Task<int> Handle(CreateProblemReportCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var reportProblem = new ReportProblemEntity
            {
                UserID = request.UserID,
                Description = request.Description,
                DateSent = DateTime.UtcNow,
            };

            context.ReportProblems.Add(reportProblem);
            await context.SaveChangesAsync(cancellationToken);

            return reportProblem.Id;
        }
    }
}
