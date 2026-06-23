using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Moderation
{
    public class ReportedPostsEntity : BaseEntity
    {
        public ReportPostEnum Reason { get; set; }
        public string? Description { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
         //user who is reporting the post
    }
}
