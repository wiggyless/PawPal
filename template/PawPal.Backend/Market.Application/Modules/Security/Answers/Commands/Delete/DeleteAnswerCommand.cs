using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Answers.Commands.Update
{
    public class DeleteAnswerCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
