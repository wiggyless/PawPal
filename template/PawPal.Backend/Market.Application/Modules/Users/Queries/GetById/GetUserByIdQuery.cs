using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetById
{
    public class GetUserByIdQuery : IRequest<GetUserByIdQueryDto>
    {
        public int Id { get; set; } 
    }
}
