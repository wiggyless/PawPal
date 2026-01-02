using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalBreed.Queries.GetById
{
    public class GetAnimalBreedByIdQuery : IRequest<GetAnimalBreedByIdQueryDto>
    {
        public required int Id { get; set; } 
    }
}
