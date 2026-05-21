using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Comments.Commands.Delete
{
    public class DeleteCommentCommand : IRequest<Unit>
    {
        public int CommentID { get; set; }  
    }
}
