using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Queries.List
{
    public sealed class ListAnimalsQueryDto
    {
        public required string Name { get; set; }
        public required string Breed { get; set; }
        public required string Gender { get; set; }
        public required string Category { get; set; }
    }
}
