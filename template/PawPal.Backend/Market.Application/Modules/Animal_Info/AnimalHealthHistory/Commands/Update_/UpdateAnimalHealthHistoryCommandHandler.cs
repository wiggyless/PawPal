using PawPal.Domain.Entities.Animal_Info;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Update_
{
    public class UpdateAnimalHealthHistoryCommandHandler(IAppDbContext context)
        : IRequestHandler<UpdateAnimalHealthHistoryCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAnimalHealthHistoryCommand request, CancellationToken cancellationToken)
        {
            var healthHistory = await context.AnimalHealthHistories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (healthHistory == null)
                throw new PawPalNotFoundException($"Animal health history with Id {request.Id} does not exist!");

            var animalAllergies = await context.AnimalsAllergies.Where(x => x.AnimalHealthHistoryId == healthHistory.Id).ToListAsync(cancellationToken);
            var animalDisabilities = await context.AnimalsDisabilities.Where(x => x.AnimalHealthHistoryId == healthHistory.Id).ToListAsync(cancellationToken);

            var animal = await context.Animals.FirstOrDefaultAsync(a => a.Id == request.AnimalId, cancellationToken);
            if(animal == null)
                throw new PawPalNotFoundException($"Animal with Id {request.AnimalId} does not exist!");

            healthHistory.AnimalId = request.AnimalId;
            healthHistory.Animal = animal;
            healthHistory.Vaccinated = request.Vaccinated;
            healthHistory.ParasiteFree = request.ParasiteFree;
            healthHistory.SpayedOrNeutered = request.SpayedOrNeutered;
            healthHistory.DietaryRestrictions = request.DietaryRestrictions;

            var allergies = new List<int>();
            var disabilities = new List<int>();

            // Validate that the user entered valid allergies and disabilities.

            if(request.Allergies.Count != 0)
            {
                foreach (var a in request.Allergies)
                {

                    var allergy =  context.Allergies.AsNoTracking().FirstOrDefault(x =>
                    x.Name.ToLower() == a.AllergyName.ToLower());
                    if (allergy == null)
                        throw new PawPalNotFoundException($"This allergy does not exist in our database!");
                    allergies.Add(allergy.Id);
                }
            }
            if(request.Disabilities.Count != 0)
            {
                foreach (var d in request.Disabilities)
                {
                    var disability =  context.Disabilities.AsNoTracking().FirstOrDefault(x =>
                    x.Name.ToLower() == d.DisabilityName.ToLower());
                    if (disability == null)
                        throw new PawPalNotFoundException($"This disability does not exist in our database!");
                    disabilities.Add(disability.Id);
                }
            }
            var toRemoveAllergies = animalAllergies.Where(x => !allergies.Contains(x.AllergyId));
            var toRemoveDisablities = animalDisabilities.Where(x => !disabilities.Contains(x.DisabilityId));
            var toBeAddedAllergies = allergies.Where(x => !animalAllergies.Select(y => y.AllergyId).Contains(x));
            var toBeAddedDisabilities = disabilities.Where(x => !animalDisabilities.Select(y => y.DisabilityId).Contains(x));

            foreach(var rmv in toRemoveAllergies)
            {
                context.AnimalsAllergies.Remove(rmv);
            }
            foreach (var dib in toRemoveDisablities)
            {
                context.AnimalsDisabilities.Remove(dib);
            }
            foreach (var all in toBeAddedAllergies)
            {
                context.AnimalsAllergies.Add(new AllergiesAnimalHealthHistory { AllergyId = all, AnimalHealthHistoryId = healthHistory.Id });
            }
            foreach (var dib in toBeAddedDisabilities)
            {
                context.AnimalsDisabilities.Add(new DisabilitiesAnimalHealthHistory { DisabilityId = dib,AnimalHealthHistoryId = healthHistory.Id });
            }
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}
