using PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Create;
using PawPal.Domain.Entities.Moderation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Commands.Create
{
    public  sealed class CreateCommentReportCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) :
        IRequestHandler<CreateCommentReportCommand, int>
    {
        public async Task<int> Handle(CreateCommentReportCommand request, CancellationToken cancellationToken)
        {
            var userSent =context.Users.AsNoTracking().FirstOrDefault(x => x.Id == request.CommentReportedByID);
            var comment = context.Comments.AsNoTracking().FirstOrDefault(x => x.Id == request.CommentID);
            if (userSent is null)
                throw new PawPalNotFoundException("User does not exist.");
            if (!currentUser.IsAuthenticated)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            if (currentUser.UserId != userSent.Id)
            {
                throw new PawPalConflictException("User is not allowed to do this action.");
            }
            if(comment is null)
            {
                throw new PawPalNotFoundException("Comment does not exist inside the databse");
            }
            var reportedComments = new ReportedCommentsEntity
            {
                CommentReportedBy = request.CommentReportedByID,
                CommentID = request.CommentID,
                DateReported = request.DateReported,
                Description = request.Description ?? "",
                Reason = request.Reason,
            };

            context.ReportedComments.Add(reportedComments);
            await context.SaveChangesAsync(cancellationToken);

            return reportedComments.Id;
        }
    }
}
