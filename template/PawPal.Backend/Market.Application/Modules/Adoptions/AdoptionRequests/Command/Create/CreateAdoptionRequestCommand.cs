using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Create
{
    public class CreateAdoptionRequestCommand : IRequest<int>
    {
        public string Status { get; set; }  
        public DateTime DateSend { get; set; }
        public int UserID { get; set; }  
        public int PostID { get; set; } 
        public int RequirementID { get; set; }  

    }
}
