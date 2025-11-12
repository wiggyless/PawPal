using PawPal.Domain.Common;
using PawPal.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Adoptions
{
    public class AdoptionStoryEntity : BaseEntity
    {
        public string? StoryText { get; set; }
        public DateTime AdoptionDate { get; set; }
        public DateTime DatePosted { get; set; }
        public string? PhotoURL { get; set; }
        public int UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}
