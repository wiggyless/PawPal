using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Posts.Queries.GetByID
{
    public class GetPostByIdQuery : IRequest<GetPostByIdQueryDto>
    {
        public int Id { get; set; } 
    }
}
