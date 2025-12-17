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

            //za svaku alergiju/disability mora se pohraniti po jedan zapis u medjutabelu
            //ako u jednom AnimalHealthHistory-u postoji vise alergija, onda se za svaku alergiju
            //sprema zapis, sto znaci ako jedna zivotinja ima alergiju na polen i neku vrstu biljke
            //onda bi izgledalo ovako:
            // AnimalHealthHistory_ID : 231 | Allergy : Pollen, Allergy_ID : 1
            // AnimalHealthHistory_ID : 231 | Allergy : Some_Plant, Allergy_ID : 2

            if (request.Allergies.Count() > 0)
            {
                foreach (var allergyByUser in request.Allergies) //poslana lista alergija od usera
                {
                    var allergy = await context.Allergies.Where(x =>
                    x.Name.ToLower().Contains(allergyByUser.AllergyName.ToLower())).FirstOrDefaultAsync(cancellationToken);
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
            await context.SaveChangesAsync(cancellationToken);
            }

            if (request.Disabilities.Count() > 0)
            {
                foreach (var disabilityByUser in request.Disabilities) //poslana lista disabilities od usera
                {
                    var disability = await context.Disabilities.Where(x =>
                    x.Name.ToLower().Contains(disabilityByUser.DisabilityName.ToLower())).FirstOrDefaultAsync(cancellationToken);
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
            await context.SaveChangesAsync(cancellationToken);
            }

            return healthHistory.Id;
        }
    }
}
