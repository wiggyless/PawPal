

using FluentValidation;

namespace PawPal.Application.Modules.Places.Cities.Queries.GetById
{
    public sealed class GetCityByQueryValidator : AbstractValidator<GetCityByIdQuery>
    {
        public GetCityByQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Must be bigger than 0");
        }
    }
}
