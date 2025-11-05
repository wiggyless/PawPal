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
            var allergy = new AllergiesEntity();
            var disability = new DisabilitiesEntity();

            if (!string.IsNullOrWhiteSpace(request.AllergyName))
            {
                 allergy = context.Allergies.Where(x => x.Name.ToLower().
                 Contains(request.AllergyName.ToLower())).FirstOrDefault();
            }

            if(!string.IsNullOrWhiteSpace(request.DisabilityName))
            {
                disability = context.Disabilities.Where(x => x.Name.ToLower()
                .Contains(request.DisabilityName.ToLower())).FirstOrDefault();
            }

            //nadji animal health history po alergiji
            var allergyHealthHistory = await context.AnimalsAllergies
                .Where(x => x.Allergy == allergy)
                .Select(x => x.AnimalHealthHistory).FirstOrDefaultAsync(cancellationToken);

            //nadji animal health history po disability

            var disabilityHealthHistory = await context.AnimalsDisabilities
                .Where(x => x.Disability == disability)
                .Select(x => x.AnimalHealthHistory).FirstOrDefaultAsync(cancellationToken);

            var q = context.AnimalHealthHistories.AsNoTracking();
            var listOfAllergies = new List<string>();
            var listOfDisabilities = new List<string>();


            if(allergyHealthHistory != null)
            {
                q = q.Where(x=> x.Id == allergyHealthHistory.Id);
                listOfAllergies = await context.AnimalsAllergies.
               Where(x => x.AnimalHealthHistoryId == allergyHealthHistory.Id)
               .Select(x => x.Allergy.Name).ToListAsync();
            }

            if (disabilityHealthHistory != null)
            {
                q = q.Where(x=> x.Id  != disabilityHealthHistory.Id);
                listOfDisabilities = await context.AnimalsDisabilities.
                Where(x => x.AnimalHealthHistoryId == disabilityHealthHistory.Id)
                .Select(x => x.Disability.Name).ToListAsync();
            }
            

            var finalResult = q.Select(x => new ListAnimalHealthHistoriesQueryDto
            {
                AnimalHealthHistoryId = x.Id,
                AnimalName = x.Animal.Name,
                AnimalCategory = x.Animal.Category.CategoryName,
                Vaccinated = x.Vaccinated,
                SpayedOrNeutered = x.SpayedOrNeutered,
                ParasiteFree = x.ParasiteFree,
                DietaryRestrictions = x.DietaryRestrictions,
                AllergyNames = listOfAllergies != null ? listOfAllergies : new List<string>(),
                DisabilityNames = listOfDisabilities != null ? listOfDisabilities : new List<string>()
            });

            return await PageResult<ListAnimalHealthHistoriesQueryDto>
                .FromQueryableAsync(finalResult, request.Paging, cancellationToken);

        }
    }
}
