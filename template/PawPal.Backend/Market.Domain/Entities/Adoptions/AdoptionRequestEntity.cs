using PawPal.Domain.Common;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Adoptions
{
    public class AdoptionRequestEntity : BaseEntity
    {
        public string ?Status { get; set; }
        public DateTime DateSent { get; set; }
        public int PostId { get; set; }
        public PostsEntity ?Post { get; set; }
        public int RequirementId { get; set; }
        public AdoptionRequirementEntity ?Requirement { get; set; }
        public int UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}
