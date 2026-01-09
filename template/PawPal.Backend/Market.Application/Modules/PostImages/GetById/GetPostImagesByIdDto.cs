using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.GetById
{
    public class GetPostImagesByIdDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public List<string> PostImages { get; set; }    
    }
}
