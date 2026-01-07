using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Allergies.Queries.GetById
{
    public class GetAllergyById : IRequest<GetAllergyByIdDto>
    {
        public int Id { get; set; }
    }
}
