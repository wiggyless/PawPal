using PawPal.Application.Modules.Security.Questions.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Answers.Commands.Update
{
    public class UpdateAnswerCommandHandler(IAppDbContext context) : IRequestHandler<UpdateAnswerCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAnswerCommand command, CancellationToken cancellationToken)
        {
            var answer = await context.SecurityAnswers.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
            if (answer == null)
            {
                throw new PawPalNotFoundException("Answer does not exist");
            }
            if (string.IsNullOrWhiteSpace(answer.Answer))
            {
                throw new PawPalNotFoundException("Answer cannot be empty");
            }
            answer.Answer = command.Answer;
            answer.QuestionID = command.QuestionID;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
