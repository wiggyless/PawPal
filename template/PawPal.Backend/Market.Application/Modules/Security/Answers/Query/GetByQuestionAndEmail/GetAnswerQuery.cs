using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Answers.Query.GetByQuestionAndEmail
{
    public class GetAnswerQuery : IRequest<GetAnswerQueryDto>
    {
        public Dictionary<int,string> Answers { get; set; }
        public string Email { get;set; }
    }
}
