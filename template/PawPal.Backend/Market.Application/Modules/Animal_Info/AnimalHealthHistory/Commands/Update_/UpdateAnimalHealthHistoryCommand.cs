using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Update_
{
    public class UpdateAnimalHealthHistoryCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public bool Vaccinated { get; set; }
        public bool SpayedOrNeutered { get; set; }
        public bool ParasiteFree { get; set; }
        public string DietaryRestrictions { get; set; }
        public List<AnimalAllergiesUpdateCommand> Allergies { get; set; } = new List<AnimalAllergiesUpdateCommand>();
        public List<AnimalDisabilitiesUpdateCommand> Disabilities { get; set; } = new List<AnimalDisabilitiesUpdateCommand>();
    }

    public class AnimalAllergiesUpdateCommand
    {
        public string? AllergyName { get; set; }
    }

    public class AnimalDisabilitiesUpdateCommand
    {
        public string? DisabilityName { get; set; }
    }
}

