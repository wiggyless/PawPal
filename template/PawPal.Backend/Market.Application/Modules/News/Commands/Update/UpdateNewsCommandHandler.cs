using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.News.Commands.Update
{
    public sealed class UpdateNewsCommandHandler(IAppDbContext ctx)
        :IRequestHandler<UpdateNewsCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var news = await ctx.News.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
            if (news == null)
                throw new PawPalNotFoundException($"News with Id {request.Id} does not exist!");

            news.Title = string.IsNullOrWhiteSpace(request.Title) ? news.Title : request.Title.Trim();
            news.Content = string.IsNullOrWhiteSpace(request.Content) ? news.Content : request.Content.Trim();

            await ctx.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
