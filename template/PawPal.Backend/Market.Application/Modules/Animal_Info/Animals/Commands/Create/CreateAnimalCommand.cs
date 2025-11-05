using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Commands.Create
{
    public class CreateAnimalCommand : IRequest<int>
    {
        public required string Name { get; set; }
        public required string Breed { get; set; }
        public required int GenderId { get; set; }
        public required int Age { get; set; }
        public required bool HasPapers { get; set; }
        public required bool ChildFriendly { get; set; }
        public required int CategoryId { get; set; }
    }
}
