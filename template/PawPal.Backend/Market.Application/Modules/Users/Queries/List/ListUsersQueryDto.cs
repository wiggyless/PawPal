using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.List
{
    public sealed class ListUsersQueryDto
    {
        public int Id { get; set; }
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public  string Email { get; set; }
        public string Username { get; set; }

    }
}
