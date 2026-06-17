using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Commands.Create
{
    public class CreateQuestionCommand : IRequest<int>
    {
        public string Question { get; set; }
    }
}
