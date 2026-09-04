namespace PawPal.Application.Modules.Posts.Commands.CreateAnimalPost
{
    public sealed class CreateAnimalPostValidator : AbstractValidator<CreateAnimalPostCommand>
    {
        public CreateAnimalPostValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(1).MaximumLength(20);
            RuleFor(x => x.Breed).NotEmpty();
            RuleFor(x => x.Age).GreaterThanOrEqualTo(0);
            RuleFor(x => x.GenderId).GreaterThan(0);
            RuleFor(x => x.CategoryId).GreaterThan(0);
            RuleFor(x => x.PostImages)
                .Must(f => f != null && f.Count > 0)
                .WithMessage("At least one image is required.");
        }
    }
}
