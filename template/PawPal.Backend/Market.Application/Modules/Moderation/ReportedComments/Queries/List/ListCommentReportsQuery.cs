using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Queries.List
{
    public class ListCommentReportsQuery : BasePagedQuery<ListCommentReportsQueryDto>
    {
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
    }
}
