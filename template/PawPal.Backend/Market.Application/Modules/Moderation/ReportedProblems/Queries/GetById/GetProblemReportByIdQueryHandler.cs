using PawPal.Application.Modules.Moderation.ReportedPosts.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedProblems.Queries.GetById
{
    internal class GetProblemReportByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<GetProblemReportById, GetProblemReportByIdQueryDto>
    {
        public async Task<GetProblemReportByIdQueryDto> Handle(GetProblemReportById request, CancellationToken cancellationToken)
        {
            var q = await context.ReportProblems.FirstOrDefaultAsync(x => request.Id == request.Id, cancellationToken);
            if (q is null)
            {
                throw new PawPalConflictException("Post report does not exist");
            }
            var user = context.Users.AsNoTracking().FirstOrDefault(x => x.Id == q.UserID);
            if (currentUser.UserId != q.UserID && currentUser.RoleId != 3)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }

            var projectedQuery = new GetProblemReportByIdQueryDto
            {
                Id = request.Id,
                UserID = q.UserID,
                Description = q.Description,
                DateSent = q.DateSent,
                Username = user.Username ?? "",
            };
            return projectedQuery;
        }
    }
}

