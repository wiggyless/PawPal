using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetByIdDisabled
{
    public class GetByIdDisabledQuery : IRequest<GetByIdDisabledQueryDto>
    {
        public int Id { get; set; } 
    }
}
