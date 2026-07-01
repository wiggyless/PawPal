using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedProblems.Queries.List
{
    public class ListProblemReportsQuery : BasePagedQuery<ListProblemReportsQueryDto>
    {
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public string? Username { get; set; }
    }
}
