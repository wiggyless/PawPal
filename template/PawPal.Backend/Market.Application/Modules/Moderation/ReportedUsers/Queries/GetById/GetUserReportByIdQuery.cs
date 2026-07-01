using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Queries.GetById
{
    public class GetUserReportByIdQuery : IRequest<GetUserReportByIdQueryDto>
    {
        public int Id { get; set; } 
    }
}
