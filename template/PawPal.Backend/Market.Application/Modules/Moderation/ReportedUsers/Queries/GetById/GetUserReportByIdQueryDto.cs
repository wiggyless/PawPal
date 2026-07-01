using PawPal.Domain.Entities.Moderation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Queries.GetById
{
    public class GetUserReportByIdQueryDto
    {
        public int ReportID { get; set; }
        public int UserReportedID { get; set; }
        public int ReportedByUserID { get; set; }
        public string? ReportedByUsername { get; set; }
        public DateTime DateSent { get; set; }
        public ReportUserEnum Reason { get; set; }
        public string Description { get; set; }
    }
}
