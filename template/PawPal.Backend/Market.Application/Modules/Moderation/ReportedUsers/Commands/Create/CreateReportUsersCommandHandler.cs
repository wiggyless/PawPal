using PawPal.API.Controllers.Moderation.ReportedPosts.Commands.Create;
using PawPal.Domain.Entities.Moderation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Create
{
    internal class CreateReportUsersCommandHandler(IAppDbContext context,IAppCurrentUser currentUser) :
        IRequestHandler<CreateReportUsersCommand, int>
    {
        public async Task<int> Handle(CreateReportUsersCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == request.ReportedUserID, cancellationToken);
            var userSent = await context.Users.FirstOrDefaultAsync(x => x.Id == request.ReportCreatedByUserID, cancellationToken);
            if (user is null)
                throw new PawPalNotFoundException("User does not exist.");
            if (userSent is null)
                throw new PawPalNotFoundException("User does not exist.");
            if (currentUser.UserId != userSent.Id)
            {
                throw new PawPalConflictException("User is not allowed to do this action.");
            }
            if (!currentUser.IsAuthenticated)
            {
                throw new PawPalConflictException("User is not allowed to do this action.");
            }
            var reportedUser = new ReportedUserEntity
            {
                ReportSentByUserID = request.ReportCreatedByUserID,
                ReportedUserID = request.ReportedUserID,
                DateSent = request.DateSent,
                Description = request.Description,
                ReportUserEnum = request.Reason,
            };

            context.ReportedUsers.Add(reportedUser);
            await context.SaveChangesAsync(cancellationToken);

            return reportedUser.Id;
        }
    }
}
