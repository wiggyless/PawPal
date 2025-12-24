using Microsoft.VisualBasic;
using PawPal.Application.Modules.Places.Cities.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Domain.Entities.Places;
namespace PawPal.Application.Modules.Users.Commands.Create
{
    public class CreateUserCommand : IRequest<int>
    {
        public required string? FirstName { get; set; }  
        public required string? LastName { get; set; }
        public required string? Email { get; set; }
        public required string? Password { get; set; }
        public required DateTime? BirthDate { get; set; }
        public required string? ProfilePictureURL { get; set; }
        public required int? RoleID { get; set; }
        public required int? City { get; set; } 
    }
}
