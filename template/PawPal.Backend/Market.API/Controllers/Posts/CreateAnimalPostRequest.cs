namespace PawPal.API.Controllers.Posts
{
    public class CreateAnimalPostRequest
    {
        // Animal
        [FromForm(Name = "name")]
        public required string Name { get; set; }
        [FromForm(Name = "breed")]
        public required string Breed { get; set; }
        [FromForm(Name = "genderId")]
        public required int GenderId { get; set; }
        [FromForm(Name = "age")]
        public required int Age { get; set; }
        [FromForm(Name = "hasPapers")]
        public required bool HasPapers { get; set; }
        [FromForm(Name = "childFriendly")]
        public required bool ChildFriendly { get; set; }
        [FromForm(Name = "categoryId")]
        public required int CategoryId { get; set; }

        // Health
        [FromForm(Name = "vaccinated")]
        public bool Vaccinated { get; set; }
        [FromForm(Name = "spayedOrNeutered")]
        public bool SpayedOrNeutered { get; set; }
        [FromForm(Name = "parasiteFree")]
        public bool ParasiteFree { get; set; }
        [FromForm(Name = "dietaryRestrictions")]
        public string? DietaryRestrictions { get; set; }
        [FromForm(Name = "allergies")]
        public List<string> Allergies { get; set; } = new();
        [FromForm(Name = "disabilities")]
        public List<string> Disabilities { get; set; } = new();

        // Images
        [FromForm(Name = "postImages")]
        public IFormFileCollection PostImages { get; set; } = default!;
    }
}
