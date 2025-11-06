using PawPal.Application.Modules.Places.Cities.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Places.Cantons.Queries_
{
    public sealed class GetCantonByIdValidator : AbstractValidator<GetCantonByIdQuery>
    {
        public GetCantonByIdValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Must be bigger than 0");
        }
    }
}
