using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Comments.Queries.List
{
    public class ListCommentsQuery : BasePagedQuery<ListCommentsQueryDto>
    {
        public int PostID { get; set; }
    }
}
