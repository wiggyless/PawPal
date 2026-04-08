using PawPal.Application.Modules.Users.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetByUsername
{
    public class GetByUsernameQuery : IRequest<GetByUsernameQueryDto>
    {
        public required string Username { get; set; }
    }
}
