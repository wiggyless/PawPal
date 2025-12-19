using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.GetByID
{
    public class GetPostByIdQueryDto
    {
        public int Id { get; set; }
        public int? AnimalID { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
