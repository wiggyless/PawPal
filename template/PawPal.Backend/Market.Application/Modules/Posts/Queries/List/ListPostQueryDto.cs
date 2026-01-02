using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.List
{
    public class ListPostQueryDto
    {
        public int PostID { get; set; }
        public string Name { get; set; }
        public int AnimalID  { get; set; }
        public int GenderID { get; set; }
        public int CategoryID { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public int? CityID { get; set; }
        public int UserID { get; set; } 
        public string? PhotoURL { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
