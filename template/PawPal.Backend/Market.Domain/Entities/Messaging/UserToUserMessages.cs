using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Messaging
{
    public class UserToUserMessages : BaseEntity
    {
        public int SenderID { get; set; }
        public int RecieverID { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }
        public bool Status { get; set; }

    }
}
