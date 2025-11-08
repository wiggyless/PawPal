using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Delete_
{
    public class DeleteAnimalHealthHistoryCommand : IRequest<Unit>
    {
        public required int Id { get; set; }
    }
}
