using PawPal.Domain.Common;
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Moderation
{
    public class ReportedCommentsEntity : BaseEntity
    {
        public ReportCommentEnum Reason { get; set; }
        public string Description { get; set; }
        public int CommentReportedBy { get; set; } 
        public int CommentID { get; set; }
        public CommentsEntity Comment { get; set; }
        public DateTime DateReported { get; set; }
        
    }
}
