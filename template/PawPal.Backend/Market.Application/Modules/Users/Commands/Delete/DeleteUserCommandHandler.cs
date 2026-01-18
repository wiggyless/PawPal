using PawPal.Application.Modules.Posts.Commands.Delete;

namespace PawPal.Application.Modules.Users.Commands.Delete
{
    public sealed class DeleteUserCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser, ISender sender) :
        IRequestHandler<DeleteUserCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (user == null) throw new PawPalNotFoundException($"User with Id {request.Id} does not exist!");

            var userPosts = await context.Posts.Where(x => x.UserId == user.Id). ToListAsync(cancellationToken);  

            foreach( var userPost in userPosts ) {
                await sender.Send(new DeletePostCommand { Id = userPost.Id }, cancellationToken); //POST BRISANJE
            };

            user.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
