using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.ListPostsByUserId
{
    public class ListPostByUserIdQueryDto
    {
        public int UserID { get;set; }
        public int PostID { get; set; } 
        public string MainImage { get; set; }  
        public string Name { get; set;}
        public int AnimalID { get; set; }
        public int? CityID { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
