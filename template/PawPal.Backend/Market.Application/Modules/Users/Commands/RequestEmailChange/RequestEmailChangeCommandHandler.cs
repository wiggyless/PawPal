using Microsoft.Extensions.Options;
using PawPal.Application.Options;
using System.Security.Cryptography;

namespace PawPal.Application.Modules.Users.Commands.RequestEmailChange
{
    public sealed class RequestEmailChangeCommandHandler(
        IAppDbContext context,
        IAppCurrentUser currentUser,
        IEmailService emailService,
        IOptions<AppUrlsOptions> appUrls)
        : IRequestHandler<RequestEmailChangeCommand, Unit>
    {
        public async Task<Unit> Handle(RequestEmailChangeCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
                throw new PawPalNotFoundException($"User with Id {request.UserId} does not exist!");

            if (currentUser.UserId != request.UserId)
                throw new PawPalConflictException("User is not allowed to do this action");

            var newEmail = request.NewEmail?.Trim();
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new PawPalConflictException("Email cannot be an empty string");

            if (string.Equals(newEmail, user.Email, StringComparison.OrdinalIgnoreCase))
                throw new PawPalConflictException("This is already your current email.");

            var emailUsed = await context.Users
                .AnyAsync(x => x.Email == newEmail && x.Id != user.Id, cancellationToken);
            if (emailUsed)
                throw new PawPalConflictException("Email is already being used!");

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            user.PendingEmail = newEmail;
            user.EmailChangeToken = token;
            user.EmailChangeTokenExpiresAt = DateTime.UtcNow.AddHours(24);

            await context.SaveChangesAsync(cancellationToken);

            var confirmUrl = $"{appUrls.Value.ClientBaseUrl}/auth/confirm-email-change?token={token}";
            await emailService.SendEmailAsync(
                newEmail!,
                "Confirm your new email address",
                $"<p>Hi {user.FirstName},</p><p>Click <a href='{confirmUrl}'>here</a> to confirm your new email address for your PawPal account.</p><p>If you didn't request this change, you can safely ignore this email.</p>"
            );

            return Unit.Value;
        }
    }
}
