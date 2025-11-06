using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Animal_Info.ManyToMany
{
    public class DisabilitiesAnimalHealthHistory : BaseEntity
    {
        public int AnimalHealthHistoryId { get; set; }
        public AnimalHealthHistoryEntity? AnimalHealthHistory { get; set; }
        public int DisabilityId { get; set; }
        public DisabilitiesEntity? Disability { get; set; }
    }
}
