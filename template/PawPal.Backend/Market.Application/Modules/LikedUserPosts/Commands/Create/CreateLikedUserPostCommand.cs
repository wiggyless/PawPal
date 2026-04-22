using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.LikedUserPosts.Commands.Create
{
    public class CreateLikedUserPostCommand : IRequest<int>
    {
        public int UserID { get; set; }
        public int PostID { get; set; }
    }
}
