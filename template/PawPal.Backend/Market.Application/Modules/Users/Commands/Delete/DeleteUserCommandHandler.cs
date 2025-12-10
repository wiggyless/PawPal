using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.Delete
{
    public sealed class DeleteUserCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser) :
        IRequestHandler<DeleteUserCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if(appCurrentUser.UserId is null)
            {
                throw new MarketBusinessRuleException("123", "User isn't authorized to do this"); // later...
            }
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (user == null) throw new PawPalNotFoundException($"User with Id {request.Id} does not exist!");
            user.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
