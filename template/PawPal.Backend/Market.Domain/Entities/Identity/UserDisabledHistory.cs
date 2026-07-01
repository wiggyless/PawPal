using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Identity
{
    public class UserDisabledHistoryEntity : BaseEntity
    {
        public int UserID { get; set; } 
        public UserEntity? User { get; set; }
        public string Reason { get; set; }
        public string? Description { get; set; }
        public DateTime DateWhenDisabled { get; set; }
    }
}
