using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Create
{
    public class CreateRequirementCommand : IRequest<int>
    {

        public required string HouseType { get; set; }
        public required string Address { get; set; }
        public int? FloorNumber { get; set; }
        public int PeopleCount { get; set; }
        public bool? ChildrenAround { get; set; }
        public bool? ElderlyAround { get; set; }
        public bool? OtherPetsAround { get; set; }
        public bool? YardAvailable { get; set; }
        public string? YardDetails { get; set; }
        public bool? PetExp { get; set; }
        public string? ExpDetails { get; set; }
        public required string PeopleAva { get; set; }
        public bool? IsGift { get; set; }
        public required string PlanedStay { get; set; }
        public decimal? SumMoney { get; set; }
        public bool? Allergy { get; set; }
        public bool? Aggressiveness { get; set; }
        public bool? TakeBack { get; set; }
        public required string HouseDetials { get; set; } 
        public string? FinalComment { get; set; }
    }
}
