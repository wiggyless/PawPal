using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Adoptions
{
    public class AdoptionRequirementEntity : BaseEntity
    {
        public string HouseType { get; set; }
        public int PeopleCount { get; set; }
        public bool ChildrenAround { get; set; }
        public bool ElderlyAround { get; set; }
        public bool OtherPetsAround { get; set; }
        public bool YardAvailable { get; set; }
    }
}
