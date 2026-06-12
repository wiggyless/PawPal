using PawPal.Domain.Common;
using PawPal.Domain.Entities.Posts;
namespace PawPal.Application.Modules.Comments.Commands.Create
{
    public sealed class CreateCommentCommandHandler(IAppDbContext context, ICommentHubService _hubService) : IRequestHandler<CreateCommentCommand,int> 
    {

        public async Task<int> Handle(CreateCommentCommand command,CancellationToken cancellationToken)
        {
            var user = context.Users.Where(x => x.Id == command.UserID).AsNoTracking().FirstOrDefault();
            var post = context.Posts.Where(x => x.Id == command.PostID).AsNoTracking().FirstOrDefault();
            if(user is null)
            {
                throw new PawPalNotFoundException("User not found");
            }
            if(post is null)
            {
                throw new PawPalNotFoundException("Post not found");
            }
            if (string.IsNullOrEmpty(command.Content))
            {
                throw new PawPalConflictException("Content cannot be empty space");
            }
            var newComment = new CommentsEntity
            {
                Content = command.Content,
                UserId = command.UserID,
                PostId = command.PostID,
                DatePosted = DateTime.UtcNow
            };

            context.Comments.Add(newComment);
            await context.SaveChangesAsync(cancellationToken);
            var commentDto = new CommentDto
            {
                CommentID = newComment.Id,
                Content = newComment.Content,
                UserID = newComment.UserId,
                DatePosted = newComment.DatePosted,
                PostID = newComment.PostId,
                Username = user.Username
               
            };
            await _hubService.SendCommentNotification(commentDto);
            return newComment.Id;
        
        }
    }
}
