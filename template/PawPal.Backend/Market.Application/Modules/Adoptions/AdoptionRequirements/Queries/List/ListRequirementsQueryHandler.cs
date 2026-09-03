using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.List
{
    public sealed class ListRequirementsQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) :
        IRequestHandler<ListRequirementsQuery,PageResult<ListRequirementsQueryDto>>
    {
        public async Task<PageResult<ListRequirementsQueryDto>> Handle(ListRequirementsQuery request,CancellationToken cancellationToken)
        {
            // This search has no per-applicant scoping (no UserId/PostId filter), so it would
            // otherwise expose every applicant's address, finances, and household details to any
            // logged-in user. Restrict it to admins until it has a real ownership-scoped filter.
            if (currentUser.RoleId != 3)
                throw new PawPalConflictException("User is not authorized for this action");

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
