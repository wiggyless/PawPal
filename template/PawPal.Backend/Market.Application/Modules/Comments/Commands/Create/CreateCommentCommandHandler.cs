using PawPal.Domain.Common;
using PawPal.Domain.Entities.Posts;
namespace PawPal.Application.Modules.Comments.Commands.Create
{
    public sealed class CreateCommentCommandHandler(IAppDbContext context,IAppCurrentUser currentUser, ICommentHubService _hubService) : IRequestHandler<CreateCommentCommand,int>
    {

        public async Task<int> Handle(CreateCommentCommand command,CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var userId = currentUser.UserId.Value;

            var user = context.Users.Where(x => x.Id == userId).AsNoTracking().FirstOrDefault();
            var post = context.Posts.Where(x => x.Id == command.PostID).AsNoTracking().FirstOrDefault();
            var userImage = context.UserImage.AsNoTracking().FirstOrDefault(x => x.UserID == userId);
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
                UserId = userId,
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
                Username = user.Username,
                PhotoURL = userImage is null ? "" : userImage.PhotoURL
            };
            await _hubService.SendCommentNotification(commentDto);
            return newComment.Id;

        }
    }
}
