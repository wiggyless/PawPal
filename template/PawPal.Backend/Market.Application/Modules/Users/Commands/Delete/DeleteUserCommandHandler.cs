namespace PawPal.Application.Modules.Users.Commands.Delete
{
    public sealed class DeleteUserCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser) :
        IRequestHandler<DeleteUserCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (user == null) throw new PawPalNotFoundException($"User with Id {request.Id} does not exist!");
            if(user.Id != appCurrentUser.UserId)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var userPosts = await context.Posts.Include(x => x.Animal).Where(x => x.UserId == user.Id).ToListAsync(cancellationToken);
            var postIds = userPosts.Select(x => x.Id).ToList();
            var postImages = await context.PostImages.Where(x => postIds.Contains(x.PostId)).ToListAsync(cancellationToken);

            foreach (var userPost in userPosts)
            {
                userPost.IsDeleted = true;
                userPost.Animal.IsDeleted = true;
            }
            foreach (var postImage in postImages)
            {
                postImage.IsDeleted = true;
            }

            user.IsDeleted = true;
            // Single SaveChangesAsync commits the user soft-delete together with all of their
            // posts/animals/images in one atomic transaction, instead of one save per post.
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
