using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Queries_.GetById
{
    public class GetAnimalCategoryByIdQuery : IRequest<GetAnimalCategoryByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
