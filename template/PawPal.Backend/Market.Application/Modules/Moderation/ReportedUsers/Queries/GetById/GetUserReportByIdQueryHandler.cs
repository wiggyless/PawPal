using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Queries.GetById
{
    public sealed class GetUserReportByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<GetUserReportByIdQuery, GetUserReportByIdQueryDto>
    {
        public async Task<GetUserReportByIdQueryDto> Handle(GetUserReportByIdQuery request, CancellationToken cancellationToken)
        {
            var q = await context.ReportedUsers.FirstOrDefaultAsync(x=>x.Id == request.Id,cancellationToken);
            if(q is null)
            {
                throw new PawPalConflictException("User report does not exist");
            }
            if (currentUser.UserId != q.ReportSentByUserID && currentUser.RoleId != 3)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var user = context.Users.AsNoTracking().FirstOrDefault(x => x.Id == q.ReportSentByUserID && !x.isUserDisabled);
            var projectedQuery = new GetUserReportByIdQueryDto
            {
                ReportID = q.Id,
                Reason = q.ReportUserEnum,
                Description = q.Description,
                UserReportedID = q.ReportedUserID,
                ReportedByUserID = q.ReportSentByUserID,
                DateSent = q.DateSent,
                ReportedByUsername = user.Username,
            };
            return projectedQuery;
        }
    }
}
