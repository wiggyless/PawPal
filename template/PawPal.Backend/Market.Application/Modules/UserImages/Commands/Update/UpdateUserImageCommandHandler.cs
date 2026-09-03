namespace PawPal.Application.Modules.UserImages.Commands.Update
{
    public class UpdateUserImageCommandHandler(IAppDbContext context, IAppCurrentUser currentUser, IFileStorageService fileStorage) : IRequestHandler<UpdateUserImageCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateUserImageCommand command,CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
                throw new PawPalConflictException("User is not allowed to do this action");
            var userId = currentUser.UserId.Value;

            var userImage = await context.UserImage.Where(x => x.UserID == userId).FirstOrDefaultAsync(cancellationToken);
            if (userImage is null)
            {
                throw new PawPalNotFoundException($"User with id {userId} not found");
            }

            var subFolder = $"users/User_{userId}";
            fileStorage.DeleteFolder(subFolder);
            userImage.PhotoURL = await fileStorage.SaveFileAsync(command.Image, subFolder, cancellationToken);
            userImage.Name = command.Image.FileName;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
