using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Queries.GetById
{
    public class GetAnimalByIdQueryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Breed { get; set; }
        public required string Gender { get; set; }
        public required int Age { get; set; }
        public required bool HasPapers { get; set; }
        public required bool ChildFriendly { get; set; }
        public required string Category { get; set; }
    }
}
