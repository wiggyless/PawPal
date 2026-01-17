using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.ListPostsByUserId
{
    public class ListPostByUserIdQueryDto
    {
        public int UserId { get;set; }
        public int PostId { get; set; } 
        public string FirstImage { get; set; }  
        public string AnimalName { get; set;}
        public int AnimalID { get; set; }
        public int? CityID { get; set; } 
    }
}
