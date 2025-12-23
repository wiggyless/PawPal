using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.GetById
{
    public class GetRequirementByIdQuery : IRequest<GetRequirementByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
