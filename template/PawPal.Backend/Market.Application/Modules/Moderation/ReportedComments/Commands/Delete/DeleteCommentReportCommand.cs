using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Commands.Delete
{
    public class DeleteCommentReportCommand : IRequest<Unit>
    {
        public int Id { get; set; } 
    }
}
