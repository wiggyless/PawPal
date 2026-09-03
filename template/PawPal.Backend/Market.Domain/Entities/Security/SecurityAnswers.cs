using PawPal.Domain.Common;
using PawPal.Domain.Entities.Identity;
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
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public int QuestionID { get; set; }
        public SecurityQuestion Question { get; set; }
    }
}
