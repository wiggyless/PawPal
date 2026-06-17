using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Commands.Update
{
    public class UpdateQuestionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string? Question { get; set; }
    }
}
