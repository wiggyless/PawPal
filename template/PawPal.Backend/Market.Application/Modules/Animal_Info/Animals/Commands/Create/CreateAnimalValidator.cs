namespace PawPal.Application.Modules.Animal_Info.Animals.Commands.Create
{
    public sealed class CreateAnimalValidator : AbstractValidator<CreateAnimalCommand>
    {
        public CreateAnimalValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(4).MaximumLength(20);
            RuleFor(x => x.Breed).NotEmpty();
            RuleFor(x=>x.Age).NotEmpty();
        }
    }
}
