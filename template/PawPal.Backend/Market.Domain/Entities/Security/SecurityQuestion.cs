using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Security
{
    public class SecurityQuestion : BaseEntity
    {
        public string Question { get; set; }
    }
}
