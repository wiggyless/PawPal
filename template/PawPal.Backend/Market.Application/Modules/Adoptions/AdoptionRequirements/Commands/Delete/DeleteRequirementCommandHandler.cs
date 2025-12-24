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
            if (user.UserId is null) throw new PawPalNotFoundException("User does not authorized to do this action");
            var req = await context.AdoptionRequirements.FirstOrDefaultAsync(x=>x.Id == command.Id,cancellationToken);
            if (req is null) throw new PawPalNotFoundException("Requirement does not exist inisde the database");
            req.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
