using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Comments.Commands.Create
{
    public class CreateCommentCommand : IRequest<int>
    {
        public int UserID { get; set; } 
        public int PostID { get; set; }
        public string Content { get; set; } 

    }
}
