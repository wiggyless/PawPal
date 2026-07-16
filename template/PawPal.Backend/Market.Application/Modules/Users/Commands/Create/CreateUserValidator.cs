using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.Create
{
    public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator() {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(4).MaximumLength(20);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(255);
            RuleFor(x => x.AboutMe).MaximumLength(255);
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
