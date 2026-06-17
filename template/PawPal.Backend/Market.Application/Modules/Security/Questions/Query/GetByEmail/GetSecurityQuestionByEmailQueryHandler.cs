using PawPal.Application.Modules.Security.Questions.Query.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Query.GetByEmail
{
    public sealed class GetSecurityQuestionByEmailQueryHandler(IAppDbContext db) :
        IRequestHandler<GetSecurityQuestionByEmailQuery, PageResult<GetSecurityQuestionByEmailQueryDto>>
    {
        public async Task<PageResult<GetSecurityQuestionByEmailQueryDto>> Handle(GetSecurityQuestionByEmailQuery
            request, CancellationToken ct)
        {
            if(request.Email is null)
            {
                throw new PawPalConflictException("Email can't be an empty filed");
            }
            var answer = db.SecurityAnswers.AsNoTracking().Where(x => x.Email == request.Email).Select(x => x.QuestionID); ;
            if(answer.AsQueryable().Count() == 0)
            {
                throw new PawPalNotFoundException("Email not found within the answers");
            }
            var question = db.SecurityQuestions.Where(x => answer.Contains(x.Id));
            var result = question.OrderBy(x => x.Question).
                Select(x => new GetSecurityQuestionByEmailQueryDto
                {
                    Id = x.Id,
                    Question = x.Question,
                });

            return await PageResult<GetSecurityQuestionByEmailQueryDto>.FromQueryableAsync
                (result, request.Paging, ct);
        }
    }
}
