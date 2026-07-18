using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Cryptography;
namespace PawPal.Application.Modules.Security.Answers.Query.GetByQuestionAndEmail
{
    public sealed class GetAnswerQueryHandler(IAppDbContext context) : IRequestHandler<GetAnswerQuery, GetAnswerQueryDto>
    {
        public async Task<GetAnswerQueryDto> Handle(GetAnswerQuery query, CancellationToken cancellationToken)
        {
            var questionIds = query.Answers.Keys.ToList();
            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == query.Email, cancellationToken);
            var question = context.SecurityQuestions.AsNoTracking().Where(x => questionIds.Contains(x.Id));
            if (user is null)
            {
                throw new PawPalNotFoundException("Invalid credentials");
            }
            if (question.Count() != query.Answers.Keys.Count) {
                throw new PawPalNotFoundException("Question does not exist");
            }
            if(query.Answers.Where(x=>string.IsNullOrWhiteSpace(x.Value)).Count() > 0 )
            {
                throw new PawPalConflictException("Answer cannot be an empty string");
            }
            Dictionary<int,string> hashStringList = [];

            hashStringList = query.Answers.Select(x => new KeyValuePair<int,string>(x.Key, ConvertToHash(x.Value))).ToDictionary();



            var answerDict = await context.SecurityAnswers
                    .Where(x => x.Email == query.Email && questionIds.Contains(x.QuestionID))
                    .ToDictionaryAsync(
                    x => x.QuestionID, 
                    x => x.Answer,     
                    cancellationToken
                    );


            var isTrue = new GetAnswerQueryDto()
            {
                // Found out what Timing Attack Explanation are so i used this cool thing where it checks every byte no 
                // matter is it wrong or right, it just goes
                isTrueAnswer = hashStringList.OrderBy(k => k.Key)
                                .Zip(answerDict.OrderBy(k => k.Key))
                                .All(pair => CryptographicOperations.FixedTimeEquals(
                                Encoding.UTF8.GetBytes(pair.First.Value),
                                Encoding.UTF8.GetBytes(pair.Second.Value)
                                ))
            };
            return isTrue;
        }
        public string ConvertToHash(string answer)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(answer);
            byte[] hashBytes = SHA256.HashData(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }

    public class CheckAnswerDto
    {
        public int QuestionID { get; set; }
        public string Answer { get; set; }
    };

}
