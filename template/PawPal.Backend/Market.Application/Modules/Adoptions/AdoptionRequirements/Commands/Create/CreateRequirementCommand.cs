using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Create
{
    public class CreateRequirementCommand : IRequest<int>
    {
       
        public string HouseType { get; set; }
        public int PeopleCount { get; set; }
        public bool ChildrenAround { get; set; }
        public bool OtherPetsAround { get; set; }
        public bool YardAvailable { get; set; }
    }
}
