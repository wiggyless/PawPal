using PawPal.Application.Modules.Moderation.ReportedPosts.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedProblems.Queries.List
{
    public class ListProblemReportsQueryHandler(IAppDbContext context)
        : IRequestHandler<ListProblemReportsQuery, PageResult<ListProblemReportsQueryDto>>
    {
        public async Task<PageResult<ListProblemReportsQueryDto>> Handle(ListProblemReportsQuery request, CancellationToken cancellationToken)
        {
            var q = context.ReportProblems.AsNoTracking();
            var user = context.Users.AsNoTracking();
            var result = q
           .GroupJoin(
           user,
           uR => uR.UserID,
           u => u.Id,
           (problemReport, usr) => new { problemReport, usr })
            .SelectMany(
           x => x.usr.DefaultIfEmpty(),
           (x, usr) => new ListProblemReportsQueryDto
           {
               Id = x.problemReport.Id,
               UserID = x.problemReport.UserID,
               Description = x.problemReport.Description,
               Username = usr.Username ?? "",
               DateSent = x.problemReport.DateSent

           }).OrderBy(x => x.DateSent);
            return await PageResult<ListProblemReportsQueryDto>.FromQueryableAsync(result, request.Paging, cancellationToken);
        }
    }
}

