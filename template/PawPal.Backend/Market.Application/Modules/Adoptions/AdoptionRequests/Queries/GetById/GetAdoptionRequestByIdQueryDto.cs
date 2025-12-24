using PawPal.Application.Modules.Users.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.GetById
{
    public class GetAdoptionRequestByIdQueryDto
    {
        public int Id { get; set; }
        public string Status { get; set; }  
        public DateTime DateSent { get; set; }  
        public int UserId { get; set; } 
        public int PostId { get; set; }
        public int RequirementId { get; set; }
    }
}
