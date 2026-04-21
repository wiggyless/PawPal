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
                    HouseType = y.HouseType,
                    Address = y.Address,
                    FloorNumber = y.FloorNumber,
                    PeopleCount = y.PeopleCount,
                    ChildrenAround = y.ChildrenAround,
                    ElderlyAround = y.ElderlyAround,
                    OtherPetsAround = y.OtherPetsAround,
                    YardAvailable = y.YardAvailable,
                    YardDetails = y.YardDetails,
                    PetExp = y.PetExp,
                    ExpDetails = y.ExpDetails,
                    PeopleAva = y.PeopleAva,
                    IsGift = y.IsGift,
                    PlanedStay = y.PlanedStay,
                    SumMoney = y.SumMoney,
                    Allergy = y.Allergy,
                    Aggressiveness = y.Aggressiveness,
                    TakeBack = y.TakeBack,
                    HouseDetials = y.HouseDetials,
                    FinalComment = y.FinalComment
                }).FirstOrDefaultAsync(cancellationToken);
            if (newReq is null) throw new PawPalNotFoundException($"Requirement with ID ->{command.Id} does not exist");
            return newReq;
        }
    }
}
