using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.News.Commands.Delete
{
    public class DeleteNewsCommandHandler (IAppCurrentUser appCurrentUser, IAppDbContext context) :
        IRequestHandler<DeleteNewsCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            var news = await context.News.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (news == null)
                throw new PawPalNotFoundException($"News with Id {request.Id} does not exist!");
            else if (appCurrentUser.RoleId != 3)
                throw new PawPalConflictException("Only administrators can delete news!");

            news.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    
    }
}
