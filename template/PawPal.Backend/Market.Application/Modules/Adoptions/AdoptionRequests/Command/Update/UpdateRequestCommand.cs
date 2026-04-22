using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Update
{
    public class UpdateRequestCommand : IRequest<Unit>
    {
        public int RequestID { get; set; }
        public string Status { get; set; }
    }
}
