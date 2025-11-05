using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.List
{
    public class ListAnimalHealthHistoriesQueryDto
    {
        public int AnimalHealthHistoryId { get; set; }
        public string AnimalName { get; set; }
        public string AnimalCategory { get; set; }
        public bool Vaccinated { get; set; }
        public bool SpayedOrNeutered { get; set; }
        public bool ParasiteFree { get; set; }
        public string? DietaryRestrictions { get; set; }
        public List<string> AllergyNames { get; set; }
        public List<string> DisabilityNames { get; set; }
    }
}
