using PawPal.Shared.Constants;
namespace PawPal.Application.Modules.Users.Commands.UpdateRole
{
    // Assigning a role (including Admin) is a distinct, authorization-gated administrative
    // action — kept separate from self-registration so a user can never grant themselves a role.
    public sealed class UpdateUserRoleCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<UpdateUserRoleCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateUserRoleCommand command, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != Roles.Admin)
            {
                throw new PawPalConflictException("Only administrators can change a user's role.");
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);
            if (user is null)
            {
                throw new PawPalNotFoundException("User does not exist");
            }

            var roleExists = await context.Roles.AnyAsync(x => x.Id == command.RoleId, cancellationToken);
            if (!roleExists)
            {
                throw new PawPalConflictException("Role does not exist");
            }

            user.RoleId = command.RoleId;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
