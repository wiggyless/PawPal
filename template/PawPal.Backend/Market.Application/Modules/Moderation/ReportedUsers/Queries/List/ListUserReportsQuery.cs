using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Queries.List
{
    public class ListUserReportsQuery : BasePagedQuery<ListUserReportsQueryDto>
    {
        public DateTime? DateSentMin { get; set; }
        public DateTime? DateSentMax { get; set; }
        public string? Username { get; set; }
    }
}
