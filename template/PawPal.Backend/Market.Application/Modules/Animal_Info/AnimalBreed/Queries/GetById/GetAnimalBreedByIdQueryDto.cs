using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalBreed.Queries.GetById
{
    public class GetAnimalBreedByIdQueryDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }    
        public int CategoryId { get; set; }   
    }
}
