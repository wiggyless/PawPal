using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Comments.Commands.Delete
{
    public class DeleteCommentCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser) : IRequestHandler<DeleteCommentCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
        {
            if (appCurrentUser is null || !appCurrentUser.IsAuthenticated)
            {
                throw new PawPalConflictException("User is not permitted to do this action");
            }
            var comment = context.Comments.Where(x=>x.Id == command.CommentID).FirstOrDefault();
            if (comment.UserId != appCurrentUser.UserId && appCurrentUser.RoleId != 3) {
                throw new PawPalConflictException("User is not permitted to do this action");
            }
            if (comment == null) {
                throw new PawPalNotFoundException("Comment does not exist");
            }
            context.Comments.Remove(comment);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        } 
    }
}
