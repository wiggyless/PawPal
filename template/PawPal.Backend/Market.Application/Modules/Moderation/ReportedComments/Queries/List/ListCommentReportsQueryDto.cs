using PawPal.Domain.Entities.Moderation;
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Queries.List
{
    public class ListCommentReportsQueryDto
    {
        public int Id { get; set; }
        public ReportCommentEnum Reason { get; set; }
        public string? Description { get; set; }
        public DateTime DateReported { get; set; }
        public ListReportedCommentEntityDto Comment { get; set; }
        public int CommentReportedByID { get; set; }
        public string? Username { get; set; }
    }
    public class ListReportedCommentEntityDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime DatePosted { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
