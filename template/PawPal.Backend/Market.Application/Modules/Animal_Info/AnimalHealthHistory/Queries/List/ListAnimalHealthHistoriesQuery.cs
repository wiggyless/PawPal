using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.List
{
    public class ListAnimalHealthHistoriesQuery : BasePagedQuery<ListAnimalHealthHistoriesQueryDto>
    {
        public string? CategoryName { get; set; }
    }
}
