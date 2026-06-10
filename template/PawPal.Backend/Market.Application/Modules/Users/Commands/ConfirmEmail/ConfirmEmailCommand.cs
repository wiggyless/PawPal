using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<ConfirmEmailCommandDto>
    {
        public string Token { get; set; }
    }
}
