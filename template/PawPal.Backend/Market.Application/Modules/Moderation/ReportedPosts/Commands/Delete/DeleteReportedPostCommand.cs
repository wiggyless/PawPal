using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedPosts.Commands.Delete
{
    public class DeleteReportedPostCommand : IRequest<Unit>
    {
        public int PostID { get; set; }
    }
}
