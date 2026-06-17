using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Answers.Commands.Update
{
    public class UpdateAnswerCommand : IRequest<Unit>
    {
        public int Id { get; set; } 
        public int QuestionID { get; set; }
        public string Answer { get; set; }
    }
}
