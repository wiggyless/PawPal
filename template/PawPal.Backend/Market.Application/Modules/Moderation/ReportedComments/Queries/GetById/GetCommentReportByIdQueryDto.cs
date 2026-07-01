using PawPal.Domain.Entities.Moderation;
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Queries.GetById
{
    public class GetCommentReportByIdQueryDto 
    {
        public int Id { get; set; } 
        public ReportCommentEnum Reason { get; set; }
        public string? Description { get; set; }
        public DateTime DateReported { get; set; }
        public ReportedCommentEntityDto Comment { get; set; }
        public int CommentReportedByID { get; set; }
        public string Username { get; set; }
    }
    public class ReportedCommentEntityDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime DatePosted { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string? PhotoURL { get; set; }
    }
}
