using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.News
{
    public class NewsEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PhotoURL { get; set; }
    }
}
