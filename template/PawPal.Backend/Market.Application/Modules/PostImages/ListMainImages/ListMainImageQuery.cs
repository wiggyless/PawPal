using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.ListMainImages
{
    public class ListMainImageQuery: IRequest<ListMainImageQueryDto>
    {
        public int PostID { get; set; }
    }
}
