using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Security
{
    public class SecurityAnswers : BaseEntity
    {
        public string Answer { get; set; }
        public string Email { get; set; }
        
        public int QuestionID { get; set; }
        public SecurityQuestion Question { get; set; }
    }
}
