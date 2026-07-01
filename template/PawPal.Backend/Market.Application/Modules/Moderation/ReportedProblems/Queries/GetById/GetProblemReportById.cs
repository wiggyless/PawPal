using PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedProblems.Queries.GetById
{
    public class GetProblemReportById : IRequest<GetProblemReportByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
