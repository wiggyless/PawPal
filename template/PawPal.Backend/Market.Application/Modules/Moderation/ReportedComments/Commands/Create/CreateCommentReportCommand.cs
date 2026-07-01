using PawPal.Domain.Entities.Moderation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Commands.Create
{
    public class CreateCommentReportCommand : IRequest<int>
    {
        public ReportCommentEnum Reason { get; set; }
        public string? Description { get; set; }
        public DateTime DateReported { get; set; }
        public int CommentID { get; set; }
        public int CommentReportedByID { get; set; }
    }
}
