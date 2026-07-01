using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedProblems.Queries.GetById
{
    public class GetProblemReportByIdQueryDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
        public DateTime DateSent { get; set; }
        public string Username { get; set; }
    }
}
