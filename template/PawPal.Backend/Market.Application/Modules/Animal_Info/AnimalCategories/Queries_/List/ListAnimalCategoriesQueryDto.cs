using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List
{
    public sealed class ListAnimalCategoriesQueryDto
    {
        public required int Id { get; set; }
        public required string CategoryName { get; set; }
    }
}
