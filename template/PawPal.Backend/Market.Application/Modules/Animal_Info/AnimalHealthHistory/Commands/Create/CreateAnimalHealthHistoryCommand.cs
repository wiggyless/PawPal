using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Create
{
    public class CreateAnimalHealthHistoryCommand : IRequest<int>
    {
        public required int AnimalId { get; set; }
        public required bool Vaccinated { get; set; } = false; //default value
        public required bool SpayedOrNeutered { get; set; } = false; //default value
        public required bool ParasiteFree { get; set; } = false; //default value
        public string? DietaryRestrictions { get; set; } = string.Empty;  //default value
        public List<AnimalAllergiesCommand> Allergies { get; set; } = new List<AnimalAllergiesCommand>();
        public List<AnimalDisabilitiesCommand> Disabilities { get; set; } = new List<AnimalDisabilitiesCommand>();
    }

    public class AnimalAllergiesCommand
    {
        public string? AllergyName { get; set; }
    }

    public class AnimalDisabilitiesCommand
    {
        public string? DisabilityName { get; set; }
    }
}
