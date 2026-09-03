using PawPal.Application.Modules.Security.Questions.Commands.Update;
using PawPal.Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PawPal.Application.Modules.Security.Answers.Commands.Update
{
    public class UpdateAnswerCommandHandler(IAppCurrentUser currentUser, IAppDbContext context) : IRequestHandler<UpdateAnswerCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAnswerCommand command, CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
            {
                throw new PawPalConflictException("User must be logged in to perform this action");
            }
            var userId = currentUser.UserId.Value;

            var questionIds = command.Answers.Keys.ToList();
            var matchingQuestionCount = await context.SecurityQuestions.AsNoTracking()
                .Where(x => questionIds.Contains(x.Id))
                .CountAsync(cancellationToken);
            if (matchingQuestionCount != command.Answers.Keys.Count)
            {
                throw new PawPalConflictException("Question does not exist");
            }

            var existingAnswers = await context.SecurityAnswers
                .Where(x => x.UserId == userId && questionIds.Contains(x.QuestionID))
                .ToListAsync(cancellationToken);

            foreach (var kvp in command.Answers)
            {
                if (string.IsNullOrWhiteSpace(kvp.Value))
                {
                    continue;
                }
                if (kvp.Value.Length < 8 || kvp.Value.Length > 30)
                {
                    throw new PawPalConflictException("Answer Length must be at least min 8 and max 30 characters long");
                }

                byte[] inputBytes = Encoding.UTF8.GetBytes(kvp.Value);
                byte[] hashBytes = SHA256.HashData(inputBytes);
                string hashString = Convert.ToHexString(hashBytes);

                var existing = existingAnswers.FirstOrDefault(x => x.QuestionID == kvp.Key);
                if (existing is not null)
                {
                    existing.Answer = hashString;
                }
                else
                {
                    context.SecurityAnswers.Add(new SecurityAnswers
                    {
                        Answer = hashString,
                        QuestionID = kvp.Key,
                        UserId = userId,
                    });
                }
            }

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
