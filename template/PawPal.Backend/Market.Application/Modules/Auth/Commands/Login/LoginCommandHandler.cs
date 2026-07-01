using PawPal.Application.Abstractions;
using PawPal.Application.Modules.Auth.Commands.Login;
using PawPal.Domain.Entities.Identity;

public sealed class LoginCommandHandler(
    IAppDbContext ctx,
    IJwtTokenService jwt,
    IPasswordHasher<UserEntity> hasher)
    : IRequestHandler<LoginCommand, LoginCommandDto>
{
    public async Task<LoginCommandDto> Handle(LoginCommand request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await ctx.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email && x.IsEnabled, ct);
        var users =  ctx.Users.Where(x => x.Email.ToLower() == email);
        if(user is null)
        {
            throw new PawPalNotFoundException("User does not exist.");
        }
        if (user.isUserDisabled)
        {
            throw new PawPalConflictException("User is disabled. Please contact support for more information");
        }
        
        var verify = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verify == PasswordVerificationResult.Failed)
            throw new PawPalConflictException("Wrong credentials.");
        /*
        if (!user.IsEmailConfirmed)
            throw new PawPalConflictException("Please verify your e-mail address before logging in.");
        */
        var tokens = jwt.IssueTokens(user);

        ctx.RefreshTokens.Add(new RefreshTokenEntity
        {
            TokenHash = tokens.RefreshTokenHash,
            ExpiresAtUtc = tokens.RefreshTokenExpiresAtUtc,
            UserId = user.Id,
            Fingerprint = request.Fingerprint
        });

        await ctx.SaveChangesAsync(ct);

        return new LoginCommandDto
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshTokenRaw,
            ExpiresAtUtc = tokens.RefreshTokenExpiresAtUtc
        };
    }
}
