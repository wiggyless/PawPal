using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.ListPostByRange
{
    public class ListPostByRangeQuery : BasePagedQuery<ListPostByRangeQueryDto>
    {
        public int[] PostIdList { get; set; } = [];
    }
}