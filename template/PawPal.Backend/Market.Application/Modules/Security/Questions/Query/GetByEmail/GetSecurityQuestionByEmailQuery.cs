using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Security.Questions.Query.GetByEmail
{
    public class GetSecurityQuestionByEmailQuery : BasePagedQuery<GetSecurityQuestionByEmailQueryDto>
    {
        public string Email { get; set; }
    }
}
