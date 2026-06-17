using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Query.GetByEmail
{
    public class GetSecurityQuestionByEmailQueryDto 
    {
        public int Id { get; set; }
        public string Question { get; set; } = "";
    }
}
