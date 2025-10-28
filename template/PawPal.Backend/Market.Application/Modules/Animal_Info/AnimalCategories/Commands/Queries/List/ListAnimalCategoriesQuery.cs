using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Queries.List
{
    public sealed class ListAnimalCategoriesQuery : BasePagedQuery<ListAnimalCategoriesQueryDto>
    {
        public string? Search { get; init; }
    }
}
