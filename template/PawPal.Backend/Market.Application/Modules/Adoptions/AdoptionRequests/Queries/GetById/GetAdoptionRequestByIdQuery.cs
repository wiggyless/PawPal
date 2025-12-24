using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.GetById
{
    public class GetAdoptionRequestByIdQuery : IRequest<GetAdoptionRequestByIdQueryDto>
    {
        public int Id { get; set; } 
    }
}
