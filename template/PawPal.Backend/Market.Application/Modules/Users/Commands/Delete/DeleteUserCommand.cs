using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.Delete
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public int? Id { get; set; }
    }
}
