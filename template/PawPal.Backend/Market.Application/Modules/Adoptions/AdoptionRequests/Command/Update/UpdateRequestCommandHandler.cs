using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Update
{
    public sealed class UpdateRequestCommandHandler(IAppDbContext context,IAppCurrentUser currentUser) : IRequestHandler<UpdateRequestCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateRequestCommand command, CancellationToken cancellationToken)
        {
            var requestAnimal = await context.AdoptionRequests.Where(x => x.Id == command.RequestID).FirstOrDefaultAsync(cancellationToken);
            if (requestAnimal is null) {
                throw new PawPalConflictException("Request does not exist");
            }
            if(requestAnimal.UserId != currentUser.UserId)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            requestAnimal.Status = command.Status;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
