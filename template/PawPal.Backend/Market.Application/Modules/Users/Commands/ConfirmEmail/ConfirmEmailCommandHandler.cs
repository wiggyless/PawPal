using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.ConfirmEmail
{
    public sealed class ConfirmEmailCommandHandler(IAppDbContext context)
    : IRequestHandler<ConfirmEmailCommand, ConfirmEmailCommandDto>
    {
        public async Task<ConfirmEmailCommandDto> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
           var user = await context.Users.FirstOrDefaultAsync(u=> u.EmailConfirmationToken == request.Token, cancellationToken);
            if (user == null)
                throw new PawPalNotFoundException("User with received confirmation token does not exist.");

            if (user.EmailConfirmationTokenExpiresAt < DateTime.UtcNow)
                throw new PawPalConflictException("Confirmation token has expired. Please request a new one.");

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null; //one-time use
            user.EmailConfirmationTokenExpiresAt = null;

            await context.SaveChangesAsync(cancellationToken);

            return new ConfirmEmailCommandDto
            {
                Message = "Email confirmed successfully. You can log in now!"
            };

        }
    }
}
