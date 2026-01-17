using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Disabilities.GetById
{
    public class GetDisabilityById : IRequest<GetDisabilityByIdDto>
    {
        public int Id { get; set; }
    }
}
