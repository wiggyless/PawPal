using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Delete
{
    public sealed class DeleteAdoptionRequestCommandHandler(IAppDbContext context,IAppCurrentUser user) 
        : IRequestHandler<DeleteAdoptionRequestCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteAdoptionRequestCommand request,CancellationToken cancellationToken)
        {
            if (user.UserId is null) throw new PawPalConflictException("User is not authorized for this action");
            var req = await context.AdoptionRequests.Include(x=>x.Requirement).Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (req == null) throw new PawPalNotFoundException("Request does not exist");
            req.IsDeleted = true;
            if(req.Requirement is not null) req.Requirement.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
