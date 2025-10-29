using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.Animals.Queries.List
{
    public sealed class ListAnimalsQuery : BasePagedQuery<ListAnimalsQueryDto>
    {
        public string? SearchName { get; init; }
        public string? SearchBreed { get; init; }
        public string? SearchGender { get; init; }
        public string? SearchCategory { get; init; }
    }
}
