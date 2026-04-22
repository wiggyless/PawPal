using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.List
{
    public sealed class ListRequirementsQueryHandler(IAppDbContext context) : 
        IRequestHandler<ListRequirementsQuery,PageResult<ListRequirementsQueryDto>>
    {
        public async Task<PageResult<ListRequirementsQueryDto>> Handle(ListRequirementsQuery request,CancellationToken cancellationToken)
        {
            var reqList = context.AdoptionRequirements.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchHouseType))
                reqList = reqList.Where(x => x.HouseType.ToLower().Contains(request.SearchHouseType.ToLower()));
            if (request.SearchPeopleCount >= 0)
                reqList = reqList.Where(x => x.PeopleCount == request.SearchPeopleCount);
            reqList = reqList.Where(x => x.OtherPetsAround == request.SearchOtherPetsAround
            && x.ChildrenAround == request.SearchChildrenAround && x.YardAvailable == request.SearchYardAvailable);
            var finalResult = reqList.OrderBy(y => y.HouseType).Select(x => new ListRequirementsQueryDto
            {
                Id = x.Id,
                HouseType = x.HouseType,
                Address = x.Address,
                FloorNumber = x.FloorNumber,
                PeopleCount = x.PeopleCount,
                ChildrenAround = x.ChildrenAround,
                ElderlyAround = x.ElderlyAround,
                OtherPetsAround = x.OtherPetsAround,
                YardAvailable = x.YardAvailable,
                YardDetails = x.YardDetails,
                PetExp = x.PetExp,
                ExpDetails = x.ExpDetails,
                PeopleAva = x.PeopleAva,
                IsGift = x.IsGift,
                PlanedStay = x.PlanedStay,
                SumMoney = x.SumMoney,
                Allergy = x.Allergy,
                Aggressiveness = x.Aggressiveness,
                TakeBack = x.TakeBack,
                HouseDetials = x.HouseDetials,
                FinalComment = x.FinalComment
            });
            return await PageResult<ListRequirementsQueryDto>.FromQueryableAsync(finalResult, request.Paging, cancellationToken);

        }
    }
}
