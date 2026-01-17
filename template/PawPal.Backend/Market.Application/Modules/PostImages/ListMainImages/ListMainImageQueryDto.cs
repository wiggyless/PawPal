using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.ListMainImages
{
    public class ListMainImageQueryDto
    {
        public int PostID { get; set; }
        public byte[] MainImage { get; set; }
    }
}
