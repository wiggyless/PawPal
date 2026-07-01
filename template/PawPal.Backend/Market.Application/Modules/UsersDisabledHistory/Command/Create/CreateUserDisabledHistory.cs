using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.UsersDisabledHistory.Command.Create
{
    public class CreateUserDisabledHistory : IRequest<int>
    { 
        public int UserID { get; set; }
        
        public string Reason { get; set; }
        public string? Description { get; set; }    
        public DateTime DateDisabled { get; set; }
    }
}
