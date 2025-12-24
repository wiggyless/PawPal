using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.News.Commands.Delete
{
    public class DeleteNewsCommand : IRequest<Unit>
    {
        public required int Id { get; set; }
    }
}
