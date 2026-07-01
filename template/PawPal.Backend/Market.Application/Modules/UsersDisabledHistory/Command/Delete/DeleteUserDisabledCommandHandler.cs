using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UsersDisabledHistory.Command.Delete
{
    public class DeleteUserDisabledCommandHandler(IAppDbContext context,IAppCurrentUser currentUser):
        IRequestHandler<DeleteUserDisabledCommand,int>
    {
        public async Task<int> Handle(DeleteUserDisabledCommand command, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != 3 || !currentUser.IsAuthenticated)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var userHistory = await context.UserDisabledHistory
                .FirstOrDefaultAsync(x => x.UserID == command.UserID, cancellationToken);
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == command.UserID, cancellationToken);
            if (user is null)
            {
                throw new PawPalNotFoundException("User does not exist");
            }
            if(userHistory is not null)
            {
                userHistory.IsDeleted = true;
            }
            user.isUserDisabled = false;
            await context.SaveChangesAsync(cancellationToken);
            return 0;
        }
    }
}
