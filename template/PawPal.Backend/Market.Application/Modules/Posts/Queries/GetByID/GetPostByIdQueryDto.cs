using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.GetByID
{
    public class GetPostByIdQueryDto
    {
        public int PostID { get; set; }
        public string Name { get; set; }
        public int AnimalID { get; set; }
        public int GenderID { get; set; }
        public int CategoryID { get; set; }
        public string Breed { get; set; }
        public int BreedID { get; set; }
        public int Age { get; set; }
        public int? CityID { get; set; }
        public int UserID { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
