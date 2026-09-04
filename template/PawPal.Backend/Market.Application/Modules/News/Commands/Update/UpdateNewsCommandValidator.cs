namespace PawPal.Application.Modules.News.Commands.Update
{
    public sealed class UpdateNewsCommandValidator : AbstractValidator<UpdateNewsCommand>
    {
        public UpdateNewsCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("Title can be at most 200 characters long.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));
        }
    }
}
