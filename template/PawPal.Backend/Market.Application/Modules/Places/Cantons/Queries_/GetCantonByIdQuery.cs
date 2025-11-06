using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Places.Cantons.Queries_
{
    public class GetCantonByIdQuery : IRequest<GetCantonByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
