using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Identity
{
    public class UserImage : BaseEntity
    {
        public int UserID { get; set; }
        public string? Name { get; set; }
        public string PhotoURL { get; set; }
    }
}
