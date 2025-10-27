

using FluentValidation;

namespace PawPal.Application.Modules.Places.Queries.GetById
{
    public sealed class GetCityByQueryValidator : AbstractValidator<GetCityByIdQuery>
    {
        public GetCityByQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Mora biti vece od nule brate dragi");
        }
    }
}
