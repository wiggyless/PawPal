namespace PawPal.Application.Modules.Users.Commands.ResendConfirmationEmail;

public sealed class ResendConfirmationEmailCommand : IRequest<ResendConfirmationEmailDto>
{
    public string Email { get; set; } = string.Empty;
}