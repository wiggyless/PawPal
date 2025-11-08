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

            var healthHistory = await context.AnimalHealthHistories.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (healthHistory == null)
                throw new PawPalNotFoundException($"Animal health history with Id {request.Id} does not exist!");

            healthHistory.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
