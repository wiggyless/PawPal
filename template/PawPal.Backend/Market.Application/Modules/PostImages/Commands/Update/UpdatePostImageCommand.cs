using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.Commands.Update
{
    public class UpdatePostImageCommand : IRequest<Unit>
    {
        [FromForm(Name = "postId")]
        public int PostId { get; set; }
        [FromForm(Name = "postImages")]
        public IFormFileCollection PostImages { get; set; }
    }
}
