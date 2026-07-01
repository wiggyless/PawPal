using PawPal.Domain.Entities.Moderation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Create
{
    public class CreateReportUsersCommand : IRequest<int>
    {
        public int ReportedUserID { get; set; }
        public int ReportCreatedByUserID { get; set; }
        public string Description { get; set; }
        public DateTime DateSent { get; set; }
        public ReportUserEnum Reason { get; set; }
    }
}
