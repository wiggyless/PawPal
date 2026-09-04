using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Create
{
    public class CreateAnimalHealthHistoryCommandHandler(IAppDbContext context)
        : IRequestHandler<CreateAnimalHealthHistoryCommand, int>
    {
        public async Task<int> Handle(CreateAnimalHealthHistoryCommand request, CancellationToken cancellationToken)
        {
            var animal = await context.Animals.Where(x => x.Id == request.AnimalId).FirstOrDefaultAsync(cancellationToken);
            if (animal == null)
                throw new PawPalNotFoundException($"Animal with Id {request.AnimalId} does not exist!");

            var requestedAllergyNames = request.Allergies
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => a.Trim().ToLower())
                .Distinct()
                .ToList();
            var requestedDisabilityNames = request.Disabilities
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .Select(d => d.Trim().ToLower())
                .Distinct()
                .ToList();

            var matchedAllergies = requestedAllergyNames.Count == 0
                ? new List<AllergiesEntity>()
                : await context.Allergies.Where(a => requestedAllergyNames.Contains(a.Name.ToLower())).ToListAsync(cancellationToken);
            if (matchedAllergies.Count != requestedAllergyNames.Count)
                throw new PawPalNotFoundException("This allergy does not exist in our database!");

            var matchedDisabilities = requestedDisabilityNames.Count == 0
                ? new List<DisabilitiesEntity>()
                : await context.Disabilities.Where(d => requestedDisabilityNames.Contains(d.Name.ToLower())).ToListAsync(cancellationToken);
            if (matchedDisabilities.Count != requestedDisabilityNames.Count)
                throw new PawPalNotFoundException("This disability does not exist in our database!");

            var healthHistory = new AnimalHealthHistoryEntity
            {
                AnimalId = animal.Id,
                Animal = animal,
                Vaccinated = request.Vaccinated,
                SpayedOrNeutered = request.SpayedOrNeutered,
                ParasiteFree = request.ParasiteFree,
                DietaryRestrictions = request.DietaryRestrictions
            };
            context.AnimalHealthHistories.Add(healthHistory);

            foreach (var allergy in matchedAllergies)
            {
                context.AnimalsAllergies.Add(new AllergiesAnimalHealthHistory
                {
                    AllergyId = allergy.Id,
                    Allergy = allergy,
                    AnimalHealthHistory = healthHistory
                });
            }

            foreach (var disability in matchedDisabilities)
            {
                context.AnimalsDisabilities.Add(new DisabilitiesAnimalHealthHistory
                {
                    DisabilityId = disability.Id,
                    Disability = disability,
                    AnimalHealthHistory = healthHistory
                });
            }

            await context.SaveChangesAsync(cancellationToken);

            return healthHistory.Id;
        }
    }
}
