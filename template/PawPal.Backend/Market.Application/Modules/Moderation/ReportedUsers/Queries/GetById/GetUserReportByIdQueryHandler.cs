using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Queries.GetById
{
    public sealed class GetUserReportByIdQueryHandler(IAppDbContext context)
        : IRequestHandler<GetUserReportByIdQuery, GetUserReportByIdQueryDto>
    {
        public async Task<GetUserReportByIdQueryDto> Handle(GetUserReportByIdQuery request, CancellationToken cancellationToken)
        {
            var q = await context.ReportedUsers.FirstOrDefaultAsync(x=>request.Id == request.Id,cancellationToken);
            var user = context.Users.AsNoTracking().FirstOrDefault(x => x.Id == q.ReportSentByUserID);
            if(q is null)
            {
                throw new PawPalConflictException("User report does not exist");
            }
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
