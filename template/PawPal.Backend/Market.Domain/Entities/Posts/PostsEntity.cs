using PawPal.Domain.Common;
using PawPal.Domain.Entities.Identity;
using PawPal.Domain.Entities.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Posts
{
    public class PostsEntity : BaseEntity
    {
        public string? Status { get; set; }
        public DateTime DateAdded { get; set; }
        public string? PhotoURL { get; set; }
        public int UserId { get; set; }
        public UserEntity ?User { get; set; }
        public int CityId { get; set; }
        public CitiesEntity? City { get; set; }
    }
}
