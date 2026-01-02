using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalBreed.Queries.List
{
    public class ListAnimalBreedQuery : BasePagedQuery<ListAnimalBreedQueryDto>
    {
        public string? SearchName { get; set; }
        public string? SearchCategoryName { get; set; }
    }
}
