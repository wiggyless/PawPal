using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Domain.Entities.Adoptions;
using PawPal.Shared.Constants;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Delete
{
    public sealed class DeleteAdoptionRequestCommandHandler(IAppDbContext context,IAppCurrentUser user)
        : IRequestHandler<DeleteAdoptionRequestCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteAdoptionRequestCommand request,CancellationToken cancellationToken)
        {
            var req = await context.AdoptionRequests.Include(x=>x.Requirement).Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (req == null) throw new PawPalNotFoundException("Request does not exist");
            if (req.UserId != user.UserId && user.RoleId != Roles.Admin) throw new PawPalConflictException("User is not authorized for this action");
            if (req.Status != AdoptionRequestStatus.Pending && user.RoleId != Roles.Admin)
                throw new PawPalConflictException("Only pending requests can be deleted; accepted or denied requests are part of the adoption history.");
            req.IsDeleted = true;
            if(req.Requirement is not null) req.Requirement.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
