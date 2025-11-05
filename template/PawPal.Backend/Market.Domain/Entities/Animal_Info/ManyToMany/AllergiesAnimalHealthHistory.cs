using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Animal_Info.ManyToMany
{
    public class AllergiesAnimalHealthHistory : BaseEntity
    {
        public int AnimalHealthHistoryId { get; set; }
        public AnimalHealthHistoryEntity? AnimalHealthHistory { get; set; }
        public int AllergyId { get; set; }
        public AllergiesEntity? Allergy { get; set; }
    }
}
