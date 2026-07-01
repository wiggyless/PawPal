using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Moderation
{
    public class ReportedUserEntity : BaseEntity
    {
        public ReportUserEnum ReportUserEnum { get; set; }
        public string? Description { get; set; }
        public int ReportedUserID { get; set; } 
        public int ReportSentByUserID { get; set; }
        public DateTime DateSent { get; set; }
    }
}
