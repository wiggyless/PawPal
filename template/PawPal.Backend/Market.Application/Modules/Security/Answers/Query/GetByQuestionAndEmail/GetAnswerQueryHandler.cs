using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace PawPal.Application.Modules.Security.Answers.Query.GetByQuestionAndEmail
{
    public sealed class GetAnswerQueryHandler(IAppDbContext context) : IRequestHandler<GetAnswerQuery, GetAnswerQueryDto>
    {
        private static readonly GetAnswerQueryDto Invalid = new() { isTrueAnswer = false };

        public async Task<GetAnswerQueryDto> Handle(GetAnswerQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.Email) ||
                query.Answers is null ||
                query.Answers.Count == 0 ||
                query.Answers.Any(x => string.IsNullOrWhiteSpace(x.Value)))
            {
                return Invalid;
            }

            var user = await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == query.Email, cancellationToken);
            if (user is null)
            {
                return Invalid;
            }

            var registeredAnswers = await context.SecurityAnswers.AsNoTracking()
                .Where(x => x.UserId == user.Id)
                .ToDictionaryAsync(x => x.QuestionID, x => x.Answer, cancellationToken);

            if (registeredAnswers.Count == 0)
            {
                return Invalid;
            }

            var submittedAnswers = query.Answers;
            var registeredQuestionIds = registeredAnswers.Keys.OrderBy(x => x).ToList();

            if (!registeredQuestionIds.SequenceEqual(submittedAnswers.Keys.OrderBy(x => x)))
            {
                return Invalid;
            }

            var isTrueAnswer = registeredQuestionIds.All(questionId =>
                CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(ConvertToHash(submittedAnswers[questionId])),
                    Encoding.UTF8.GetBytes(registeredAnswers[questionId])));

            return new GetAnswerQueryDto { isTrueAnswer = isTrueAnswer };
        }

        public string ConvertToHash(string answer)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(answer);
            byte[] hashBytes = SHA256.HashData(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}
