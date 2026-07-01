using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedProblems.Commands.Create
{
    public class CreateProblemReportCommand : IRequest<int>
    {
        public string Description { get; set; } 
        public int UserID { get; set; }
        public DateTime DateSent { get; set; }
    }
}
