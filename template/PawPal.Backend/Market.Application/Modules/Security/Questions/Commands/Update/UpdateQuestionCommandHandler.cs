using PawPal.Application.Modules.Security.Questions.Commands.Create;
using PawPal.Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Commands.Update
{
    public class UpdateQuestionCommandHandler(IAppDbContext context, IAppCurrentUser currentUser): IRequestHandler<UpdateQuestionCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateQuestionCommand command, CancellationToken cancellationToken)
        {
            if (currentUser.RoleId != 3)
                throw new PawPalConflictException("Only administrators can update security questions.");

            var question = await context.SecurityQuestions.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
            if(question == null)
            {
                throw new PawPalNotFoundException("Question does not exist");
            }
            if (string.IsNullOrWhiteSpace(question.Question))
            {
                throw new PawPalNotFoundException("Question cannot be empty");
            }
            question.Question = command.Question;
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
