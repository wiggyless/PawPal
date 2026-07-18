namespace PawPal.Application.Modules.Users.Commands.ConfirmEmailChange
{
    public sealed class ConfirmEmailChangeCommandHandler(IAppDbContext context)
        : IRequestHandler<ConfirmEmailChangeCommand, ConfirmEmailChangeCommandDto>
    {
        public async Task<ConfirmEmailChangeCommandDto> Handle(ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.EmailChangeToken == request.Token, cancellationToken);
            if (user == null)
                throw new PawPalNotFoundException("User with received email change token does not exist.");

            if (user.EmailChangeTokenExpiresAt < DateTime.UtcNow)
                throw new PawPalConflictException("Email change link has expired. Please request a new one.");

            if (string.IsNullOrWhiteSpace(user.PendingEmail))
                throw new PawPalConflictException("There is no pending email change for this account.");

            user.Email = user.PendingEmail;
            user.PendingEmail = null;
            user.EmailChangeToken = null;
            user.EmailChangeTokenExpiresAt = null;

            foreach (var rt in user.RefreshTokens.Where(x => !x.IsRevoked))
            {
                rt.IsRevoked = true;
                rt.RevokedAtUtc = DateTime.UtcNow;
            }

            await context.SaveChangesAsync(cancellationToken);

            return new ConfirmEmailChangeCommandDto
            {
                Message = "Your email has been updated. Please log in again."
            };
        }
    }
}
