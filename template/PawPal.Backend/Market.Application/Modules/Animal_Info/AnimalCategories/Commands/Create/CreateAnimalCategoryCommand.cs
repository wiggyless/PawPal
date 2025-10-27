using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Create
{
    public class CreateAnimalCategoryCommand : IRequest<int>
    {
        public required string CategoryName { get; set; }
    }
}
