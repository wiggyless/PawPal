namespace PawPal.Application.Modules.Users.Commands.ConfirmEmailChange
{
    public class ConfirmEmailChangeCommand : IRequest<ConfirmEmailChangeCommandDto>
    {
        public string Token { get; set; }
    }
}
