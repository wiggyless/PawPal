using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.GetById
{
    public class GetAnimalHealthHistoryByIdQuery : IRequest<GetAnimalHealthHistoryByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
