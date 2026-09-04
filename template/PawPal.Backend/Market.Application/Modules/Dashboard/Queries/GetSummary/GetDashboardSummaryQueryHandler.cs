using PawPal.Domain.Entities.Adoptions;
using PawPal.Domain.Entities.Posts;
using PawPal.Shared.Constants;

namespace PawPal.Application.Modules.Dashboard.Queries.GetSummary
{
    public class GetDashboardSummaryQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<GetDashboardSummaryQuery, GetDashboardSummaryQueryDto>
    {
        public async Task<GetDashboardSummaryQueryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != Roles.Admin)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }

            return new GetDashboardSummaryQueryDto
            {
                ActiveListings = await context.Posts.AsNoTracking()
                    .CountAsync(p => p.Status == PostStatus.Active, cancellationToken),
                PendingAdoptionRequests = await context.AdoptionRequests.AsNoTracking()
                    .CountAsync(r => r.Status == AdoptionRequestStatus.Pending, cancellationToken),
                ReportedPosts = await context.ReportedPosts.AsNoTracking().CountAsync(cancellationToken),
                ReportedUsers = await context.ReportedUsers.AsNoTracking().CountAsync(cancellationToken),
                ReportedComments = await context.ReportedComments.AsNoTracking().CountAsync(cancellationToken),
                ReportedProblems = await context.ReportProblems.AsNoTracking().CountAsync(cancellationToken),
            };
        }
    }
}
