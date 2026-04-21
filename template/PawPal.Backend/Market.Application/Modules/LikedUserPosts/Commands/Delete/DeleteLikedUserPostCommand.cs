using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.LikedUserPosts.Commands.Delete
{
    public class DeleteLikedUserPostCommand : IRequest<Unit>
    {
        public int UserId { get; set; }
        public int PostId { get; set; }

    }
}
