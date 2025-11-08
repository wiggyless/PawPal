using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.List
{
    public class ListAnimalHealthHistoriesQueryHandler(IAppDbContext context)
        : IRequestHandler<ListAnimalHealthHistoriesQuery, PageResult<ListAnimalHealthHistoriesQueryDto>>
    {
        public async Task<PageResult<ListAnimalHealthHistoriesQueryDto>> Handle(ListAnimalHealthHistoriesQuery request, CancellationToken cancellationToken)
        {
            var query = context.AnimalHealthHistories.AsNoTracking();

            if(!string.IsNullOrEmpty(request.CategoryName))
                query = query.Where(x=> x.Animal.Category.CategoryName.ToLower().Contains(request.CategoryName.ToLower()));

            var finalResult = query.OrderBy(x => x.Animal.Name)
                .Select(x => new ListAnimalHealthHistoriesQueryDto
                {
                    AnimalHealthHistoryId = x.Id,
                    AnimalName = x.Animal.Name,
                    AnimalCategory = x.Animal.Category.CategoryName,
                    Vaccinated = x.Vaccinated,
                    SpayedOrNeutered = x.SpayedOrNeutered,
                    ParasiteFree = x.ParasiteFree,
                    DietaryRestrictions = x.DietaryRestrictions,
                    Allergies = context.AnimalsAllergies.Where(y => y.AnimalHealthHistoryId == x.Id)
                    .Select(z => new AllergyDto
                    {
                        AllergyName = z.Allergy.Name
                    }).ToList(),
                    Disabilities = context.AnimalsDisabilities.Where(y => y.AnimalHealthHistoryId == x.Id)
                    .Select(z => new DisabilityDto
                    {
                        DisabilityName = z.Disability.Name
                    }).ToList()
                });

            return await PageResult<ListAnimalHealthHistoriesQueryDto>
                    .FromQueryableAsync(finalResult, request.Paging, cancellationToken);
        }
    }
}

