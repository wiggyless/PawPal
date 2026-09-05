using PawPal.Domain.Entities.News;
using PawPal.Shared.Constants;

namespace PawPal.Application.Modules.News.Commands.Create
{
    public sealed class CreateNewsCommandHandler(IAppDbContext context, IAppCurrentUser user, IFileStorageService fileStorage)
        : IRequestHandler<CreateNewsCommand, int>
    {
        public async Task<int> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            if(user.RoleId != Roles.Admin) //only admins can create news
                throw new ValidationException("Only administrators can create news.");

            string? photoUrl = null;
            if (request.Photo is not null)
            {
                photoUrl = await fileStorage.SaveFileAsync(request.Photo, "news_photos", cancellationToken);
            }

            var news = new NewsEntity
            {
                Title = request.Title,
                Content = request.Content,
                PublishedAt = DateTime.UtcNow,
                PhotoURL = photoUrl
            };

            context.News.Add(news);
            await context.SaveChangesAsync(cancellationToken);
            return news.Id;
        }
    }
}
