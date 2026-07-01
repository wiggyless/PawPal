using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedPosts.Queries.GetById
{
    public class GetReportedPostByIdQuery : IRequest<GetReportedPostByIdQueryDto>
    {
        public int Id { get; set; } 
    }
}
