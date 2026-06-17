using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Answers.Commands.Create
{
    public class CreateAnswerCommand : IRequest<int>
    {
        public Dictionary<int,string> Answers { get; set; } = new Dictionary<int,string>();
        public string Email { get; set; }
    }
}
