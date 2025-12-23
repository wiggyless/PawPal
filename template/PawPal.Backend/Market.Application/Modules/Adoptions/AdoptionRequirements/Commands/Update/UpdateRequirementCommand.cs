using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Update
{
    public class UpdateRequirementCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int PeopleCount { get; set; }
        public string HouseType { get; set; }
        public bool HasChildrenAround { get; set; }
        public bool OtherPetsAround { get; set; }
    }
}
