using PawPal.Application.Modules.Users.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.UpdatePassword
{
    public sealed class UpdatePasswordCommandHandler(IAppDbContext context) : IRequestHandler<UpdatePasswordCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Email == request.Email, cancellationToken);
            if (user == null)
            {
                throw new PawPalNotFoundException($"User with Email {request.Email} does not exist!");
            }
            var password = request.NewPassword?.Trim();
            if (string.IsNullOrWhiteSpace(password)) {
                throw new PawPalNotFoundException("Password cannot be an empty string");
            }
            var hasher = new PasswordHasher<UserEntity>();
            user.PasswordHash = hasher.HashPassword(null, password);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}