using PawPal.Domain.Entities.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.Update
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureURL { get; set; }
        public DateTime Date { get; set; }
        public int CityId { get; set; }
    }
}
