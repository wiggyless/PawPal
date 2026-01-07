using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Allergies.Queries.GetById
{
    public class GetAllergyByIdDto 
    {
        public int AllergyID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
    }
}
