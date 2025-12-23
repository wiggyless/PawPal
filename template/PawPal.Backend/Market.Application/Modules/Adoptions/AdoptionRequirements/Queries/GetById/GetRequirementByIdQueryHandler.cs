using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.GetById
{
    public sealed class GetRequirementByIdQueryHandler(IAppDbContext context) :
        IRequestHandler<GetRequirementByIdQuery,GetRequirementByIdQueryDto>
    {
        public async Task<GetRequirementByIdQueryDto> Handle(GetRequirementByIdQuery command,CancellationToken cancellationToken)
        {
            var newReq = await context.AdoptionRequirements.
                Where(x => x.Id == command.Id).
                Select(y => new GetRequirementByIdQueryDto
                {
                    Id = y.Id,
                    ChildrenAround = y.ChildrenAround,
                    PeopleCount = y.PeopleCount,
                    OtherPetsAround = y.OtherPetsAround,
                    YardAvailable = y.YardAvailable,
                    HouseType = y.HouseType,
                }).FirstOrDefaultAsync(cancellationToken);
            if (newReq is null) throw new PawPalNotFoundException($"Requirement with ID ->{command.Id} does not exist");
            return newReq;
        }
    }
}
