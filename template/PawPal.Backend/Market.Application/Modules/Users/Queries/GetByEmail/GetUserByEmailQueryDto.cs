using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetByEmail
{
    public class GetUserByEmailQueryDto
    {
        public required string Email { get; set; }
        public bool Exists { get; set; }
    }
}
