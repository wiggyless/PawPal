using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedProblems.Commands.Delete
{
    public class DeleteProblemReportCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
