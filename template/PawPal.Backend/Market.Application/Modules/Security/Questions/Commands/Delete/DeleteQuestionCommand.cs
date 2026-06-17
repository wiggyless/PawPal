using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Commands.Delete
{
    public class DeleteQuestionCommand : IRequest<Unit>
    {
        public int Id { get; set; } 
    }
}
