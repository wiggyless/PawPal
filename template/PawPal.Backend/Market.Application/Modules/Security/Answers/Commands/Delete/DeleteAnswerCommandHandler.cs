using PawPal.Application.Modules.Security.Questions.Commands.Delete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Answers.Commands.Update
{
    public class DeleteAnswerCommandHandler(IAppCurrentUser user, IAppDbContext context) :
        IRequestHandler<DeleteAnswerCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAnswerCommand command, CancellationToken cancellationToken)
        {
            if (user is null)
            {
                throw new PawPalConflictException("User cannot do this action");
            }
            var answer = await context.SecurityAnswers.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
            if (answer == null)
            {
                throw new PawPalNotFoundException("Question does not exist");
            }
            context.SecurityAnswers.Remove(answer);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
