using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Common
{
    public class CommentDto
    {
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
        public string Username { get; set;  }
        public string PhotoURL { get; set; }
    }
}
