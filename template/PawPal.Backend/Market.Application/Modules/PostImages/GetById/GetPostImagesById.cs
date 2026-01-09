using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.GetById
{
    public class GetPostImagesById : IRequest<GetPostImagesByIdDto>
    {
        public int PostId { get; set; } 
    }
}
