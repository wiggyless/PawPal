namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.CreateWithRequirement
{
    /// <summary>
    /// Creates the adoption requirement and the adoption request together in a single
    /// transactional call, so a client can never leave an orphaned requirement behind
    /// (which happened when the frontend chained two separate create calls).
    /// </summary>
    public class CreateAdoptionRequestWithRequirementCommand : IRequest<int>
    {
        public int PostID { get; set; }

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
