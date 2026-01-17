using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.GetByIdFile
{
    public class GetImagesPostByIdFileQueryDto
    {
        public int PostId { get; set; }
        public List<byte[]> PostImages { get; set; } 

    }
}
