using PawPal.Application.Modules.Security.Answers.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Answers.Commands.Update
{
    public sealed class UpdateAnswerValidator : AbstractValidator<UpdateAnswerCommand>
    {
        public UpdateAnswerValidator()
        {
            RuleFor(x => x.Answers)
                .Must(x => x.Count == 3)
                .WithMessage("You must provide exactly 3 security answers.");
        }
    }

    public class StringValidator : AbstractValidator<KeyValuePair<int, string>>
    {
        public StringValidator()
        {
            RuleFor(model => model.Value)
                .Length(8, 30)
                .WithMessage("\nYour answer must be between 8 and 30 characters long. You entered {TotalLength} characters.\n")
                .OverridePropertyName(string.Empty)
                .Unless(model=>string.IsNullOrEmpty(model.Value));

        }
    }
}
