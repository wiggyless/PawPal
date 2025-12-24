using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Delete
{
    public class DeleteAdoptionRequestCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
