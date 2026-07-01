using PawPal.API.Controllers.Moderation.ReportedPosts.Commands.Create;
using PawPal.Domain.Entities.Moderation;

namespace PawPal.Application.Modules.Moderation.ReportedPosts.Commands.Create
{
    public sealed class CreateReportPostHandler(IAppDbContext context,IAppCurrentUser currentUser) :
        IRequestHandler<CreateReportPostCommand, int>
    {
        public async Task<int> Handle(CreateReportPostCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == request.UserID,cancellationToken);
            if (user is null)
                throw new PawPalNotFoundException("User does not exist.");
            if (!currentUser.IsAuthenticated)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == request.PostID, cancellationToken);
            if (post is null)
                throw new PawPalNotFoundException("Post does not exist.");

            var reportedPost = new ReportedPostsEntity
            {
                PostID = request.PostID,
                UserID = request.UserID,
                Reason = request.Reason,
                Description = request.Description,
                DateSent = DateTime.UtcNow,
            };

            context.ReportedPosts.Add(reportedPost);
            await context.SaveChangesAsync(cancellationToken);

            return reportedPost.Id;
        }
    }
}
