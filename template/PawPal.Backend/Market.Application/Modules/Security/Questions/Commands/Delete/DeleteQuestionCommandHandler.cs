using PawPal.Application.Modules.Security.Questions.Commands.Create;
using PawPal.Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Commands.Delete
{
    public class DeleteQuestionCommandHandler(IAppCurrentUser user,IAppDbContext context) : IRequestHandler<DeleteQuestionCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteQuestionCommand command, CancellationToken cancellationToken)
        {
            if(user.RoleId != 3)
            {
                throw new PawPalConflictException("Only administrators can delete security questions.");
            }
            var question = await context.SecurityQuestions.FirstOrDefaultAsync(x=>x.Id == command.Id,cancellationToken);
            if(question == null)
            {
                throw new PawPalNotFoundException("Question does not exist");
            }
            context.SecurityQuestions.Remove(question);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
