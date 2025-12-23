using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.List
{
    public class ListRequirementsQuery : BasePagedQuery<ListRequirementsQueryDto>
    {
        public string? SearchHouseType { get; set; }  
        public int? SearchPeopleCount { get; set; }   
        public bool SearchChildrenAround { get; set; }
        public bool SearchOtherPetsAround { get; set; }
        public bool SearchYardAvailable { get; set; }   
    }
}
