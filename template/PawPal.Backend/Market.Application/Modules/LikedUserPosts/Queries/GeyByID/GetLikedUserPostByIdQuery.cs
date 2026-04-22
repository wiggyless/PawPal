using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.LikedUserPosts.Queries.GeyByID
{
    public class GetLikedUserPostByIdQuery : IRequest<GetLikedUserPostByQueryDto>
    {
        public int UserId { get; set; }
        public int PostId { get; set; } 
    }
}
