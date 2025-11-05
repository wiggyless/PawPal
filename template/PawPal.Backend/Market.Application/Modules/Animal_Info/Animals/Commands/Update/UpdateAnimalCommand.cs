using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Commands.Update
{
    public sealed class UpdateAnimalCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public bool HasPapers { get; set; }
        public bool ChildFriendly { get; set; }
        public string Category { get; set; }
    }
}
