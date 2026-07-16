using PawPal.Application.Modules.Moderation.ReportedPosts.Queries.List;
using PawPal.Application.Modules.Users.Queries.List;
using PawPal.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Queries.List
{
    public class ListUserReportsQueryHandler(IAppDbContext context)
        : IRequestHandler<ListUserReportsQuery, PageResult<ListUserReportsQueryDto>>
    {
        public async Task<PageResult<ListUserReportsQueryDto>> Handle(ListUserReportsQuery request, CancellationToken cancellationToken)
        {
            var q = context.ReportedUsers.AsNoTracking();
            var user = context.Users.Where(x=>!x.isUserDisabled).AsNoTracking().AsQueryable();
            if (request.DateSentMin is not null && request.DateSentMax is not null)
            {
                q = q.Where(x => x.DateSent >= request.DateSentMin && x.DateSent <= request.DateSentMax);
            }
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                user = user.Where(x => x.Username.ToLower().Contains(request.Username.ToLower()));
            }
            var result = q
            .GroupJoin(
            user,
            uR => uR.ReportSentByUserID,
            u => u.Id,
            (userReport, usr) => new { userReport, usr })
             .SelectMany(
            x => x.usr.DefaultIfEmpty(),      
            (x, usr) => new ListUserReportsQueryDto
            {
            ReportID = x.userReport.Id,
            UserReportedID = x.userReport.ReportedUserID,
            ReportedByUserID = x.userReport.ReportSentByUserID,
            Description = x.userReport.Description,
            Reason = x.userReport.ReportUserEnum,
            ReportedByUsername = usr.Username  ,
            DateSent = x.userReport.DateSent,
            }).OrderBy(x=>x.DateSent);
            return await PageResult<ListUserReportsQueryDto>.FromQueryableAsync(result, request.Paging, cancellationToken);
        }
    }
}
