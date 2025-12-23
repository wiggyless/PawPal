using Microsoft.AspNetCore.Http;
using PawPal.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.News
{
    public class NewsEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
        public string PhotoURL { get; set; }

         [NotMapped]
        public IFormFile File { get; set; }
    }
}
