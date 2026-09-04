namespace PawPal.Application.Modules.News.Commands.Create
{
    public sealed class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
    {
        public CreateNewsCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(200).WithMessage("Title can be at most 200 characters long.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content cannot be empty.");
        }
    }
}
