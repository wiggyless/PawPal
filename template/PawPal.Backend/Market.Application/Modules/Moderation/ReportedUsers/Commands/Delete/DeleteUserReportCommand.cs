using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Delete
{
    public class DeleteUserReportCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
