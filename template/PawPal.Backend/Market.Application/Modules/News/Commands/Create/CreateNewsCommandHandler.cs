using PawPal.Domain.Entities.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.News.Commands.Create
{
    public sealed class CreateNewsCommandHandler(IAppDbContext context, IAppCurrentUser user)
        : IRequestHandler<CreateNewsCommand, int>
    {
        public async Task<int> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            if(user.RoleId != 3) //only admins can create news
                throw new ValidationException("Only administrators can create news.");

            if (string.IsNullOrWhiteSpace(request.Title))
            throw new ValidationException("Title cannot be empty.");

           if(string.IsNullOrWhiteSpace(request.Content))
            throw new ValidationException("Content cannot be empty.");

            var news = new NewsEntity
            {
                Title = request.Title,
                Content = request.Content,
                PublishedAt = DateTime.UtcNow,
                PhotoURL = request.PhotoURL 
            };

            context.News.Add(news);
            await context.SaveChangesAsync(cancellationToken);
            return news.Id;
        }
    }
}
