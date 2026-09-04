using PawPal.Shared.Models;
using System.Collections.Generic;

namespace PawPal.Application.Modules.Posts.Commands.CreateAnimalPost
{
    public class CreateAnimalPostCommand : IRequest<int>
    {
        // Animal
        public required string Name { get; set; }
        public required string Breed { get; set; }
        public required int GenderId { get; set; }
        public required int Age { get; set; }
        public required bool HasPapers { get; set; }
        public required bool ChildFriendly { get; set; }
        public required int CategoryId { get; set; }

        // Health
        public bool Vaccinated { get; set; }
        public bool SpayedOrNeutered { get; set; }
        public bool ParasiteFree { get; set; }
        public string? DietaryRestrictions { get; set; }
        public List<string> Allergies { get; set; } = new();
        public List<string> Disabilities { get; set; } = new();

        // Images
        public List<FileUpload> PostImages { get; set; } = default!;
    }
}
