using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Posts
{
    public class PostImagesEntity : BaseEntity
    {
        public int PostId { get; set; } 
        public List<string>? PhotoURL { get; set; }
    }
}
