using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.List
{
    public sealed class ListAdoptionRequestQueryHandler(IAppDbContext context) :
        IRequestHandler<ListAdoptionRequestQuery,PageResult<ListAdoptionRequestQueryDto>>
    {
        public async Task<PageResult<ListAdoptionRequestQueryDto>> Handle(ListAdoptionRequestQuery request,CancellationToken cancellationToken)
        {
            var reqList = context.AdoptionRequests.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchStatus))
                reqList = reqList.Where(x => x.Status.ToLower().Contains(request.SearchStatus.ToLower()));
            reqList.Where(x => x.DateSent == request.SearchDateSent);
            var finalList = reqList.OrderBy(y => y.DateSent).Select(x => new ListAdoptionRequestQueryDto{
                DateSent = x.DateSent,
                Status = x.Status,
                RequirementId = x.RequirementId,
            });
            return await PageResult<ListAdoptionRequestQueryDto>.FromQueryableAsync(finalList, request.Paging, cancellationToken);
        }
    }
}
