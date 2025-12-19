using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Commands.Delete
{
    public class DeletePostCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
