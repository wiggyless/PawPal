using PawPal.Application.Modules.Security.Questions.Commands.Create;
using PawPal.Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
namespace PawPal.Application.Modules.Security.Answers.Commands.Create
{
    public class CreateAnswerCommandHandler(IAppCurrentUser currentUser, IAppDbContext context) : IRequestHandler<CreateAnswerCommand, int>
    {
        public async Task<int> Handle(CreateAnswerCommand command, CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
            {
                throw new PawPalConflictException("User must be logged in to perform this action");
            }
            var userId = currentUser.UserId.Value;

            var questionIds = command.Answers.Keys.ToList();
            var question = context.SecurityQuestions.AsNoTracking().Where(x => questionIds.Contains(x.Id));
            if (question.Count() != command.Answers.Keys.Count)
            {
                throw new PawPalConflictException("Question does not exist");
            }

            var alreadyAnswered = await context.SecurityAnswers.AsNoTracking()
                .Where(x => x.UserId == userId && questionIds.Contains(x.QuestionID))
                .Select(x => x.QuestionID)
                .ToListAsync(cancellationToken);
            if (alreadyAnswered.Count > 0)
            {
                throw new PawPalConflictException("An answer has already been registered for one or more of these questions");
            }

            SecurityAnswers newAnswer = null!;
            for (int i =0;i<command.Answers.Values.Count;i++)
            {
                if (string.IsNullOrWhiteSpace(command.Answers.Values.ElementAt(i)))
                {
                    throw new PawPalConflictException("Answer cannot be empty");
                }
                byte[] inputBytes = Encoding.UTF8.GetBytes(command.Answers.Values.ElementAt(i));
                byte[] hashBytes = SHA256.HashData(inputBytes);

                string hashString = Convert.ToHexString(hashBytes);

                newAnswer = new SecurityAnswers
                {
                    Answer = hashString,
                    QuestionID = command.Answers.Keys.ElementAt(i),
                    UserId = userId,
                };
                context.SecurityAnswers.Add(newAnswer);
                await context.SaveChangesAsync(cancellationToken);
            }
            return newAnswer.Id;
        }
    }
}
