using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.LikedUserPosts.Queries.List
{
    public class ListLikedUserPostDto
    {
        public int UserId { get; set; }
        public List<int> PostList { get; set; }
    }
}
