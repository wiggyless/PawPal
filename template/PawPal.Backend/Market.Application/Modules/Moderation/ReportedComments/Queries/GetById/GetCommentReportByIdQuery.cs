using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedComments.Queries.GetById
{
    public class GetCommentReportByIdQuery : IRequest<GetCommentReportByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
