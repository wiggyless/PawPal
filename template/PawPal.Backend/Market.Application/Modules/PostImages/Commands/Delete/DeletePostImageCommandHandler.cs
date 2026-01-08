using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.Commands.Delete
{
    public class DeletePostImageCommandHandler(IAppCurrentUser user,IAppDbContext context)
        : IRequestHandler<DeletePostImageCommand,Unit>
    {
        public async Task<Unit> Handle(DeletePostImageCommand command,CancellationToken cancellationToken)
        {
            if (user is null) throw new MarketBusinessRuleException("123", "User isn't authorized to do this");
            var postImage = await context.PostImages.Where(x => x.PostId == command.PostId).FirstOrDefaultAsync(cancellationToken);
            if (postImage is null) throw new PawPalNotFoundException("Post does not contain images");
            if(command.PostImages.Count == 0)
            {
                postImage.IsDeleted = true;
            }
            else
            {
                postImage.PhotoURL = command.PostImages;
                await context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}
