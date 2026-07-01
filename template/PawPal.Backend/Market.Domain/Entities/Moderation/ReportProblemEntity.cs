using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Moderation
{
    public class ReportProblemEntity : BaseEntity
    {
        public string Description { get; set; }
        public int UserID { get; set; } 
        public DateTime DateSent { get; set; }
    }
}
