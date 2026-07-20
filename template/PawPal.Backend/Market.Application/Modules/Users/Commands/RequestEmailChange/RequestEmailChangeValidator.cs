namespace PawPal.Application.Modules.Users.Commands.RequestEmailChange
{
    public sealed class RequestEmailChangeValidator : AbstractValidator<RequestEmailChangeCommand>
    {
        public RequestEmailChangeValidator()
        {
            RuleFor(x => x.NewEmail).NotEmpty().EmailAddress();
        }
    }
}
