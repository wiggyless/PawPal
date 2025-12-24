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
        public string UserName { get; set; }
        public AnimalEntity Animal { get; set; }
        public string? CityName { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
