using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UsersDisabledHistory.Command.Delete
{
    public class DeleteUserDisabledCommand : IRequest<int>
    {
        public int UserID { get; set; }
    }
}
