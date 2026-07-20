using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Update
{
    public sealed class UpdateRequirementCommandHandler(IAppDbContext context, IAppCurrentUser user) : IRequestHandler<UpdateRequirementCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateRequirementCommand command,CancellationToken cancellationToken)
        {
            if (command.PeopleCount < 0) throw new PawPalConflictException("Invalid number of people");
            var req = await context.AdoptionRequirements.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
            if (req is null) throw new PawPalConflictException("Requirement does not exist inside the database");
            var owningRequest = await context.AdoptionRequests.FirstOrDefaultAsync(x => x.RequirementId == command.Id, cancellationToken);
            if (owningRequest is not null && owningRequest.UserId != user.UserId && user.RoleId != 3)
                throw new PawPalConflictException("User is not authorized for this action");
            req.PeopleCount = command.PeopleCount;
            req.OtherPetsAround = command.OtherPetsAround;
            req.HouseType = command.HouseType;
            req.ChildrenAround = command.HasChildrenAround;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
