using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.ListHistory
{
    public class ListAdoptionRequestHistoryQueryDto
    {
    public int RequestId { get; set; }
    public string Status { get; set; }
    public DateTime DateSent { get; set; }
    public int RequirementId { get; set; }
    public int UserID { get; set; }
    public int PostID { get; set; }
    public string Gender { get; set; }
    public string Breed { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Canton { get; set; }
}
}
