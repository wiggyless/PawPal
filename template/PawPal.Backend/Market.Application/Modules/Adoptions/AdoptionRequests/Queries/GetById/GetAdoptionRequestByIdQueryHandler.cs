using PawPal.Domain.Entities.Adoptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Shared.Constants;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.GetById
{
    public sealed class GetAdoptionRequestByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) :
        IRequestHandler<GetAdoptionRequestByIdQuery,GetAdoptionRequestByIdQueryDto>
    {
        public async Task<GetAdoptionRequestByIdQueryDto> Handle(GetAdoptionRequestByIdQuery request,CancellationToken cancellationToken)
        {
            var adoptionReq = await context.AdoptionRequests.
                Include(x=>x.Post).
                Where(y => y.Id == request.Id).
                Select(x => new
                {
                    Dto = new GetAdoptionRequestByIdQueryDto
                    {
                        Status = x.Status == AdoptionRequestStatus.Accepted ? "Accepted"
                            : x.Status == AdoptionRequestStatus.Denied ? "Denied"
                            : "Pending",
                        DateSent = x.DateSent,
                        UserId = x.UserId,
                        PostId = x.PostId,
                        RequirementId = x.RequirementId,
                        AnimalID = x.Post.AnimalID,
                    },
                    PostOwnerId = x.Post.UserId,
                }).FirstOrDefaultAsync(cancellationToken);
            if (adoptionReq is null) throw new PawPalNotFoundException($"Adoption request with {request.Id} does not exist");
            if (adoptionReq.Dto.UserId != currentUser.UserId && adoptionReq.PostOwnerId != currentUser.UserId && currentUser.RoleId != Roles.Admin)
                throw new PawPalConflictException("User is not allowed to do this action");
            return adoptionReq.Dto;
        }
    }
}
