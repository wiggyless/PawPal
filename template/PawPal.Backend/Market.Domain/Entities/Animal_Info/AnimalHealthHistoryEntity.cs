using PawPal.Domain.Common;
using PawPal.Domain.Entities.Animal_Info.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Animal_Info
{
    public class AnimalHealthHistoryEntity : BaseEntity
    {
        public int AnimalId { get; set; }
        public AnimalEntity? Animal { get; set; }
        public bool Vaccinated { get; set; }
        public bool SpayedOrNeutered { get; set; }
        public bool ParasiteFree { get; set; }
        public string? DietaryRestrictions { get; set; }
        List<AllergiesAnimalHealthHistory> AnimalAllergies { get; set; } = [];
        List<DisabilitiesAnimalHealthHistory> AnimalDisabilities{ get; set; } = [];
    }
}
