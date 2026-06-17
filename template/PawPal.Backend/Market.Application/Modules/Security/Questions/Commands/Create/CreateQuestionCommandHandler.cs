using PawPal.Application.Modules.Disabilities.Command.Create;
using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Commands.Create
{
    public class CreateQuestionCommandHandler(IAppDbContext context) : IRequestHandler<CreateQuestionCommand, int>
    {
        public async Task<int> Handle(CreateQuestionCommand command, CancellationToken cancellationToken)
        {
            var newQuestion = new SecurityQuestion
            {
                Question = command.Question,
            };
            context.SecurityQuestions.Add(newQuestion);
            await context.SaveChangesAsync(cancellationToken);
            return newQuestion.Id;
        }
    }
}
