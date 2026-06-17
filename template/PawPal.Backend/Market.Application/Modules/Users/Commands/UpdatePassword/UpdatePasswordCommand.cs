using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.UpdatePassword
{
    public class UpdatePasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
