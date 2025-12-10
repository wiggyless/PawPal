using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Identity
{
    public class RolesEntity : BaseEntity
    {
        public string? RoleName { get; set; }
    }
}
