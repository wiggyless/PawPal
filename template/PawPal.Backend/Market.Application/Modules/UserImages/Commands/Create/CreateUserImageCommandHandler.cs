using PawPal.Domain.Entities.Posts;

namespace PawPal.Application.Modules.UserImages.Commands.Create
{
    public class CreateUserImageCommandHandler(IAppDbContext context, IAppCurrentUser currentUser, IFileStorageService fileStorage) : IRequestHandler<CreateUserImageCommand, int>
    {
        public async Task<int> Handle(CreateUserImageCommand command,CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
                throw new PawPalConflictException("User is not allowed to do this action");
            var userId = currentUser.UserId.Value;

            var user = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync(cancellationToken);
            if (user is null)
                throw new PawPalNotFoundException($"User with ID:{userId} not found");

            var photoUrl = await fileStorage.SaveFileAsync(command.Image, $"users/User_{userId}", cancellationToken);

            var newPostImages = new UserImage
            {
                UserID = userId,
                PhotoURL = photoUrl,
                Name = command.Image.FileName,
            };
            context.UserImage.Add(newPostImages);
            await context.SaveChangesAsync(cancellationToken);
            return newPostImages.Id;
        }
    }
}
