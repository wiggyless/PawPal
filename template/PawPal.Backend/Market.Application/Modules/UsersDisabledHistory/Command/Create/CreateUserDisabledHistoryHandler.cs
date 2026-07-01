using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UsersDisabledHistory.Command.Create
{
    public class CreateUserDisabledHistoryHandler(IAppDbContext context,IAppCurrentUser currentUser)
        :IRequestHandler<CreateUserDisabledHistory,int>
    {
        public async Task<int> Handle(CreateUserDisabledHistory command,CancellationToken cancellationToken)
        {
            if(!currentUser.IsAuthenticated || currentUser.RoleId != 3)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            };
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == command.UserID, cancellationToken);
            if(user is null)
            {
                throw new PawPalNotFoundException("User is not found");
            }
            var result = new UserDisabledHistoryEntity
            {
                UserID = command.UserID,
                DateWhenDisabled = command.DateDisabled,
                Description = command.Description,
                Reason = command.Reason,
            };
            user.isUserDisabled = true;
            await context.UserDisabledHistory.AddAsync(result,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return result.Id;
        }
    }
}
