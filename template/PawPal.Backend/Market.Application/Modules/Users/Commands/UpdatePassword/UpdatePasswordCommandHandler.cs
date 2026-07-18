using PawPal.Application.Modules.Users.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.UpdatePassword
{
    public sealed class UpdatePasswordCommandHandler(IAppDbContext context,IAppCurrentUser currentUser,IPasswordHasher<UserEntity> hash) : IRequestHandler<UpdatePasswordCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Email == request.Email, cancellationToken);
            if (user == null)
            {
                throw new PawPalNotFoundException($"User with Email {request.Email} does not exist!");
            }
            if (!request.PasswordRecovery)
            {
                if (currentUser.Email != request.Email)
                {
                    throw new PawPalConflictException("User is not allowed to do this action");
                }
                var verify = hash.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
                if (verify == PasswordVerificationResult.Failed)
                    throw new PawPalConflictException("Incorrect password");
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