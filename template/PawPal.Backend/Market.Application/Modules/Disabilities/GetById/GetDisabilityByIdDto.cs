using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Disabilities.GetById
{
    public class GetDisabilityByIdDto
    {
        public int DisabilityID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
