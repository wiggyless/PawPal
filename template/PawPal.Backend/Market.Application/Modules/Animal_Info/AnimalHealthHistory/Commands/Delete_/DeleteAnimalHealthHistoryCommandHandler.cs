using PawPal.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Delete_
{
    public class DeleteAnimalHealthHistoryCommandHandler(IAppDbContext context, IAppCurrentUser user)
        : IRequestHandler<DeleteAnimalHealthHistoryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAnimalHealthHistoryCommand request, CancellationToken cancellationToken)
        {
            if (user.UserId is null)
                throw new MarketBusinessRuleException("123", "User isn't authorized to do this.");

            var healthHistory = await context.AnimalHealthHistories.Where(x => x.AnimalId == request.AnimalId).FirstOrDefaultAsync(cancellationToken);
            if (healthHistory == null)
                throw new PawPalNotFoundException($"Animal health history for animal with Id {request.AnimalId} does not exist!");

            // Only the owner of the post this health history belongs to (or an admin) may delete it.
            var owningPost = await context.Posts.AsNoTracking()
                .FirstOrDefaultAsync(p => p.AnimalID == request.AnimalId, cancellationToken);
            if (user.RoleId != 3 && (owningPost is null || owningPost.UserId != user.UserId))
                throw new PawPalConflictException("User is not allowed to do this action");

            healthHistory.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
