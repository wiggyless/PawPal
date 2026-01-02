using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalBreed.Commands.Create
{
    public class CreateAnimalBreedCommand : IRequest<int>
    {
        public required string Name { get; set; }    
        public required int CategoryId { get; set; }
    }
}
