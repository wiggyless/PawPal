using PawPal.Domain.Common;
using PawPal.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Posts
{
    public class LikedUserPosts : BaseEntity
    {
        public int PostId { get; set; }
        public PostsEntity Post { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public DateTime DateLiked { get; set; }
    }
}
