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

            var animal = await context.Animals.FirstOrDefaultAsync(a => a.Id == request.AnimalId, cancellationToken);
            if(animal == null)
                throw new PawPalNotFoundException($"Animal with Id {request.Id} does not exist!");

            healthHistory.AnimalId = request.AnimalId;
            healthHistory.Animal = animal;
            healthHistory.Vaccinated = request.Vaccinated;
            healthHistory.ParasiteFree = request.ParasiteFree;
            healthHistory.SpayedOrNeutered = request.SpayedOrNeutered;
            healthHistory.DietaryRestrictions = request.DietaryRestrictions;

            var allergies = new List<AllergiesEntity>();
            var disabilities = new List<DisabilitiesEntity>();

            //validacija da li je korisnik unio ispravne alergije i poremecaje
            foreach(var a in request.Allergies)
            {
                var allergy = await context.Allergies.Where(x =>
                x.Name.ToLower().Contains(a.AllergyName.ToLower())).FirstOrDefaultAsync(cancellationToken);
                if (allergy == null)
                    throw new PawPalNotFoundException($"This allergy does not exist in our database!");
                allergies.Add(allergy);
            }

            foreach(var d in request.Disabilities)
            {
                var disability = await context.Disabilities.Where(x =>
               x.Name.ToLower().Contains(d.DisabilityName.ToLower())).FirstOrDefaultAsync(cancellationToken);
                if (disability == null)
                    throw new PawPalNotFoundException($"This disability does not exist in our database!");
                disabilities.Add(disability);
            }

            //brisanje starih pohranjenih alergija i poremecaja iz baze, jer ako dodamo nove a ne izbrisemo stare
            //onda ce nam se i stari pokazivati
            if (request.Allergies.Count > 0)
            {
                var animalAllergies = await context.AnimalsAllergies.Where(x => x.AnimalHealthHistoryId == healthHistory.Id).ToListAsync(cancellationToken);
                foreach (var aa in animalAllergies)
                    aa.IsDeleted = true;
                await context.SaveChangesAsync(cancellationToken);
            }

            if (request.Disabilities.Count > 0)
            {
                var animalDisabilities = await context.AnimalsDisabilities.Where(x => x.AnimalHealthHistoryId == healthHistory.Id).ToListAsync(cancellationToken);
                foreach (var ad in animalDisabilities)
                    ad.IsDeleted = true;
                await context.SaveChangesAsync(cancellationToken);
            }

            //dodavanje novih
            foreach (var a in allergies)
            {
                var newAnimalAllergy = new AllergiesAnimalHealthHistory
                {
                    AllergyId = a.Id,
                    Allergy = a,
                    AnimalHealthHistoryId = healthHistory.Id,
                    AnimalHealthHistory = healthHistory
                };
                context.AnimalsAllergies.Add(newAnimalAllergy);
            }

            await context.SaveChangesAsync(cancellationToken);

            foreach (var d in disabilities)
            {
                var newAnimalDisability = new DisabilitiesAnimalHealthHistory
                {
                    DisabilityId = d.Id,
                    Disability = d,
                    AnimalHealthHistoryId = healthHistory.Id,
                    AnimalHealthHistory = healthHistory
                };
                context.AnimalsDisabilities.Add(newAnimalDisability);
            }

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}
