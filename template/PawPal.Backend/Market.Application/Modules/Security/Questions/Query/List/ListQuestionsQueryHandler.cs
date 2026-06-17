using PawPal.Application.Modules.Places.Cantons.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Query.List
{
    public sealed class ListQuestionsQueryHandler(IAppDbContext db) :
        IRequestHandler<ListQuestionsQuery, PageResult<ListQuestionsQueryDto>>
    {
        public async Task<PageResult<ListQuestionsQueryDto>> Handle(ListQuestionsQuery
            request, CancellationToken ct)
        {
            var questions = db.SecurityQuestions.AsQueryable().AsNoTracking();

            var result = questions.OrderBy(x => x.Question).
                Select(x => new ListQuestionsQueryDto
                {
                    Id = x.Id,
                    Question = x.Question,
                });

            return await PageResult<ListQuestionsQueryDto>.FromQueryableAsync
                (result, request.Paging, ct);
        }
    }
}
