using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Answers.Commands.Create
{
    public sealed class CreateAnswerValidator : AbstractValidator<CreateAnswerCommand>
    {
        public CreateAnswerValidator() {
            RuleFor(x => x.Answers).Must(x => x.Count == 3);
            RuleForEach(x => x.Answers).SetValidator(new StringValidator());
        }
    }
    public class StringValidator : AbstractValidator<KeyValuePair<int, string>>
    {
        public StringValidator()
        {
            RuleFor(model => model.Value)
                .NotEmpty().MaximumLength(30).MinimumLength(8).NotEmpty();
        }
    }
}
