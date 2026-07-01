using PawPal.Domain.Entities.Moderation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedPosts.Queries.GetById
{
    public class GetReportedPostByIdQueryDto
    {
        public int Id { get; set; } 
        public ReportPostEnum Reason { get; set; }
        public string? Description { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public DateTime DateSent { get; set; }
        public string Username { get; set; }
    }
}
