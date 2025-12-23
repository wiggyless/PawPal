using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Delete
{
    public class DeleteRequirementCommand : IRequest<Unit>
    {
        public int Id { get; set; } 
    }
}
