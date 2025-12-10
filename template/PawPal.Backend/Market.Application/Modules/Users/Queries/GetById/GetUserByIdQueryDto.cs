using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetById
{
    public class GetUserByIdQueryDto
    {
        public int Id { get; set; }
        public required string? FirstName { get; set; }
        public required string? LastName { get; set; }   
        public required string? Email { get; set; }
        public required DateTime? DateTime { get; set; }
    }
}
