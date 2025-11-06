using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.GetById
{
    public class GetAnimalHealthHistoryByIdQueryHandler(IAppDbContext context)
        : IRequestHandler<GetAnimalHealthHistoryByIdQuery, GetAnimalHealthHistoryByIdQueryDto>
    {
        public async Task<GetAnimalHealthHistoryByIdQueryDto> Handle(GetAnimalHealthHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var listOfAllergies = await context.AnimalsAllergies.
                Where(x => x.AnimalHealthHistoryId == request.Id)
                .Select(x => x.Allergy).ToListAsync();

            var listOfDisabilities = await context.AnimalsDisabilities.
                Where(x=> x.AnimalHealthHistoryId == request.Id)
                .Select(x=> x.Disability).ToListAsync();

            var healthHistory = await context.AnimalHealthHistories.
                Where(x => x.Id == request.Id)
                .Select(x => new GetAnimalHealthHistoryByIdQueryDto
                {
                    AnimalHealthHistoryId = x.Id,
                    AnimalName = x.Animal.Name,
                    AnimalCategory = x.Animal.Category.CategoryName,
                    Vaccinated = x.Vaccinated,
                    SpayedOrNeutered = x.SpayedOrNeutered,
                    ParasiteFree = x.ParasiteFree,
                    DietaryRestrictions = x.DietaryRestrictions
                }).FirstOrDefaultAsync(cancellationToken);

            if (healthHistory == null) throw new PawPalNotFoundException($"Animal Health History wit Id {request.Id} does not exist!");

            foreach (var a in listOfAllergies)
            {
                var allergy = new GetAllergyFromAnimalDto
                {
                    AllergyName = a.Name,
                    AllergyDescription = a.Description
                };
                healthHistory.AnimalAllergies.Add(allergy);
            }

            foreach(var d in listOfDisabilities)
            {
                var disability = new GetDisabilityFromAnimalDto
                {
                    DisabilityName = d.Name,
                    DisabilityDescription = d.Description
                };
                healthHistory.AnimalDisabilities.Add(disability);
            }

            return healthHistory;
        }
    }
}
