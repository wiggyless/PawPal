using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.ListPostsByUserId
{
    public class ListPostByUserIdQuery : BasePagedQuery<ListPostByUserIdQueryDto>
    {
        public int UserId { get; set; }
    }
}
