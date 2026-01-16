using PawPal.Application.Modules.Allergies.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Disabilities.List
{
    public class ListDisabilityQuery : BasePagedQuery<ListDisabilityQueryDto>
    {
        public string? SearchName { get; set; }
    }
}
