using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Domain.Entities.Adoptions;
namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Delete
{
    public sealed class DeleteRequirementCommandHandler(IAppDbContext context,IAppCurrentUser user) : IRequestHandler<DeleteRequirementCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteRequirementCommand command,CancellationToken cancellationToken)
        {
            var req = await context.AdoptionRequirements.FirstOrDefaultAsync(x=>x.Id == command.Id,cancellationToken);
            if (req is null) throw new PawPalNotFoundException("Requirement does not exist in the database");
            var owningRequest = await context.AdoptionRequests.FirstOrDefaultAsync(x => x.RequirementId == command.Id, cancellationToken);
            if (owningRequest is not null && owningRequest.UserId != user.UserId && user.RoleId != 3)
                throw new PawPalConflictException("User is not authorized to do this action");
            req.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
