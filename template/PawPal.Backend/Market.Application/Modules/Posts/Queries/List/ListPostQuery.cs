using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.List
{
    public class ListPostQuery : BasePagedQuery<ListPostQueryDto>
    {
        public string? SearchCityName { get; set; }
        public string? SearchCategoryName { get; set; }
        public string? SearchBreed { get; set; }
        public string? SearchGender { get; set; }
        public DateTime? SearchDateAddedMax { get; set; }
        public DateTime? SearchDateAddedMin { get; set; }
    }
}
