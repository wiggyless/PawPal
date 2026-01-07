using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Allergies.Queries.List
{
    public class ListAllergyQuery : BasePagedQuery<ListAllergyQueryDto>
    {
        public string? SearchName { get; set; }
    }
}
