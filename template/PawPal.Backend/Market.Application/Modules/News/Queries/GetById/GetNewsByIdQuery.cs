using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.News.Queries.GetById
{
    public class GetNewsByIdQuery : IRequest<GetNewsByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
