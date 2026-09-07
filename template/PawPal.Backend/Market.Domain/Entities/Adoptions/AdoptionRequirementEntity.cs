using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Adoptions
{
    public class AdoptionRequirementEntity : BaseEntity
    {
        public string HouseType { get; set; }

        public string Address { get; set; }

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

        public string PlanedStay { get; set; }

        public decimal? SumMoney { get; set; }

        public bool? Allergy { get; set; }

        public bool? Aggressiveness { get; set; }

        public bool? TakeBack { get; set; }

        public string HouseDetials { get; set; }

        public string? FinalComment { get; set; }

        // Set at creation from the authenticated user. A requirement has no owner-only
        // relation until an AdoptionRequest links to it via RequirementId, so this is the
        // only way to authorize Update/Delete on a not-yet-attached requirement.
        public int? CreatedByUserId { get; set; }

    }
}
