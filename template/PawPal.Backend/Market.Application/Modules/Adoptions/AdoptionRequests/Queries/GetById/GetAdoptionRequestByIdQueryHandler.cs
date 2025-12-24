using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.GetById
{
    public sealed class GetAdoptionRequestByIdQueryHandler(IAppDbContext context) : 
        IRequestHandler<GetAdoptionRequestByIdQuery,GetAdoptionRequestByIdQueryDto>
    {
        public async Task<GetAdoptionRequestByIdQueryDto> Handle(GetAdoptionRequestByIdQuery request,CancellationToken cancellationToken)
        {
            var adoptionReq = await context.AdoptionRequests.
                Where(y => y.Id == request.Id).
                Select(x => new GetAdoptionRequestByIdQueryDto
                {
                    Status = x.Status,
                    DateSent = x.DateSent,
                    UserId = x.UserId,
                    PostId = x.PostId,
                    RequirementId = x.RequirementId
                }).FirstOrDefaultAsync(cancellationToken);
            if (adoptionReq is null) throw new PawPalNotFoundException($"Adoption request with {request.Id} does not exist");
            return adoptionReq;
        }
    }
}
