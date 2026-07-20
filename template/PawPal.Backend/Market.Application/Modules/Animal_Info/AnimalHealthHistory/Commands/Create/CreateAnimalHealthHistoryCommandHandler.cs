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
            await context.SaveChangesAsync(cancellationToken);

   

            if (request.Allergies.Count() > 0)
            {
                foreach (var allergyByUser in request.Allergies) 
                {
                    if(allergyByUser != "")
                    {
                        var allergy = await context.Allergies.Where(x =>
                    x.Name.ToLower() == allergyByUser.ToLower()).FirstOrDefaultAsync(cancellationToken);
                        if (allergy == null)
                            throw new PawPalNotFoundException($"This allergy does not exist in our database!");

                        var animalAllergies = new AllergiesAnimalHealthHistory
                        {
                            AllergyId = allergy.Id,
                            Allergy = allergy,
                            AnimalHealthHistoryId = healthHistory.Id,
                            AnimalHealthHistory = healthHistory
                        };
                        context.AnimalsAllergies.Add(animalAllergies);
                    }
                }
            await context.SaveChangesAsync(cancellationToken);
            }

            if (request.Disabilities.Count() > 0)
            {
                foreach (var disabilityByUser in request.Disabilities) 
                {
                    if(disabilityByUser != "")
                    {
                        var disability = await context.Disabilities.Where(x =>
                        x.Name.ToLower() == disabilityByUser.ToLower()).FirstOrDefaultAsync(cancellationToken);
                        if (disability == null)
                            throw new PawPalNotFoundException($"This disability does not exist in our database!");

                        var animalDisabilities = new DisabilitiesAnimalHealthHistory
                        {
                            DisabilityId = disability.Id,
                            Disability = disability,
                            AnimalHealthHistoryId = healthHistory.Id,
                            AnimalHealthHistory = healthHistory
                        };
                        context.AnimalsDisabilities.Add(animalDisabilities);
                    }

                }
            await context.SaveChangesAsync(cancellationToken);
            }

            return healthHistory.Id;
        }
    }
}
