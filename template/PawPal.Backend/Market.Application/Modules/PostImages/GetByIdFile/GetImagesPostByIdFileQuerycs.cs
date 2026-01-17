using Microsoft.AspNetCore.Hosting;
using PawPal.Application.Modules.PostImages.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.GetByIdFile
{
    public class GetImagesPostByIdFileQuerycs : IRequest<GetImagesPostByIdFileQueryDto>
    {
        public int PostId { get; set; }
    }
}
