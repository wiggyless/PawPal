using PawPal.Application.Modules.Security.Questions.Commands.Update;
using PawPal.Domain.Entities.Security;
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
            var questionIds = command.Answers.Keys.ToList();
            var question = context.SecurityQuestions.AsNoTracking().Where(x => questionIds.Contains(x.Id));
            var answers = await context.SecurityAnswers
            .Where(x => x.Email == command.Email).OrderBy(x=>x.QuestionID)
            .ToDictionaryAsync(
            x => x.QuestionID,
            x => x.Answer,
            cancellationToken
            );
            var email = context.Users.FirstOrDefault(x => string.Compare(x.Email, command.Email) == 0 ? true : false);
            if (question.Count() != command.Answers.Keys.Count)
            {
                throw new PawPalConflictException("Question does not exist");
            }
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                throw new PawPalConflictException("Email cannot be empty space");
            }
            if (email is null)
            {
                throw new PawPalNotFoundException("Email does not exist");
            }
            var orderedCommand = command.Answers.OrderBy(x => x.Key);
            var counter = 0;
            foreach (var kvp in orderedCommand) { 
                var temp = answers.FirstOrDefault(x=>x.Key == kvp.Key);
                if(temp.Value != null)
                {
                   if(kvp.Value != "" || !string.IsNullOrEmpty(kvp.Value))
                   {
                     if(kvp.Value.Length <8 || kvp.Value.Length > 30)
                        {
                            throw new
                                PawPalConflictException
                                ("Answer Length must be at least min 8 and max 30 characters long");
                        }
                     temp = new KeyValuePair<int,string>(kvp.Key, kvp.Value);
                   }
                }
                else
                {
                    var tmp = answers.ElementAt(counter++);
                    tmp = new KeyValuePair<int, string>(kvp.Key, kvp.Value);
                }
            }
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
    }
    }
}
