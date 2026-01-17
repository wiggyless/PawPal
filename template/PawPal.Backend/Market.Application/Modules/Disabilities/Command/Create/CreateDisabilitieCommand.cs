using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Disabilities.Command.Create
{
    public class CreateDisabilitieCommand : IRequest<int>
    {
        public required string Name { get; set; }
        public string Description { get; set; }
    }
}
