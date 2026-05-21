using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Comments.Queries.List
{
    public class ListCommentsQueryDto 
    {
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public string Username { get; set; }
    }
}
