using PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.ListHistory
{
    public class ListAdoptionRequestHistoryQuery : BasePagedQuery<ListAdoptionRequestHistoryQueryDto>
    {
        public int? UserID { get; set; }
        public bool Sent { get; set; }
        public string? SearchStatus { get; set; }
        public DateTime? SearchDateSent { get; set; }
    }
}
