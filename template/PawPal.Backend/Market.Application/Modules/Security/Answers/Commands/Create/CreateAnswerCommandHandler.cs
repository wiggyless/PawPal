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
    public class CreateAnswerCommandHandler(IAppDbContext context) : IRequestHandler<CreateAnswerCommand, int>
    {
        public async Task<int> Handle(CreateAnswerCommand command, CancellationToken cancellationToken)
        {
            var questionIds = command.Answers.Keys.ToList();
            var question = context.SecurityQuestions.AsNoTracking().Where(x => questionIds.Contains(x.Id));
            var email = context.Users.FirstOrDefault(x => string.Compare(x.Email, command.Email) == 0 ? true : false);
            if(question.Count() != command.Answers.Keys.Count)
            {
                throw new PawPalConflictException("Question does not exist");
            }
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                throw new PawPalConflictException("Email cannot be empty space");
            }
            if(email is null)
            {
                throw new PawPalNotFoundException("Email does not exist");
            }
            var newAnswer = new SecurityAnswers();
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
                    Email = command.Email,
                };
                context.SecurityAnswers.Add(newAnswer);
                await context.SaveChangesAsync(cancellationToken);
            }
            return newAnswer.Id;
        }
    }
}
