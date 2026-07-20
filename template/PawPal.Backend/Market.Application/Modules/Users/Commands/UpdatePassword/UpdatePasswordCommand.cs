using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Commands.UpdatePassword
{
    public class UpdatePasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        [MinLength(8)]
        public string NewPassword { get; set; }
        public bool PasswordRecovery { get; set; }
        public string? CurrentPassword { get; set; }
        public Dictionary<int, string>? Answers { get; set; }
    }
}
