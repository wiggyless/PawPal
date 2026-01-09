using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.Commands.Update
{
    public class UpdatePostImageCommand : IRequest<Unit>
    {
        public int PostId { get; set; } 
        public List<string> PostImages { get; set; }    
    }
}
