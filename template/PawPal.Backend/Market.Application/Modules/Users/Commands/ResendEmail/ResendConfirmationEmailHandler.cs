using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PawPal.Application.Abstractions;
using PawPal.Application.Options;
using PawPal.Domain.Entities.Identity;
using System.Security.Cryptography;

namespace PawPal.Application.Modules.Users.Commands.ResendConfirmationEmail;

public sealed class ResendConfirmationEmailCommandHandler(
    IAppDbContext context,
    IEmailService emailService,
    IOptions<AppUrlsOptions> appUrls)
    : IRequestHandler<ResendConfirmationEmailCommand, ResendConfirmationEmailDto>
{
    public async Task<ResendConfirmationEmailDto> Handle(
        ResendConfirmationEmailCommand request,
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
            throw new PawPalNotFoundException("No account found with that email address.");

        if (user.IsEmailConfirmed)
            throw new PawPalConflictException("This email is already confirmed.");

        var newToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        user.EmailConfirmationToken = newToken;
        user.EmailConfirmationTokenExpiresAt = DateTime.UtcNow.AddHours(24);

        await context.SaveChangesAsync(cancellationToken);

        var confirmUrl = $"{appUrls.Value.ClientBaseUrl}/auth/confirm-email?token={newToken}";
        await emailService.SendEmailAsync(
            user.Email!,
            "Confirm your email",
            $"<p>Hi {user.FirstName},</p><p>Click <a href='{confirmUrl}'>here</a> to confirm your account.</p>"
        );

        return new ResendConfirmationEmailDto
        {
            Message = "A new confirmation email has been sent. Please check your inbox."
        };
    }
}