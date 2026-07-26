using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetByUsername
{
    public class GetByUsernameQueryDto
    {
        public required string Username { get; set; }
        public bool Exists { get; set; }
    }
}
