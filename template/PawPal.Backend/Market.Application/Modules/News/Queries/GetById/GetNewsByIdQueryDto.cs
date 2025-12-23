using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.News.Queries.GetById
{
    public class GetNewsByIdQueryDto
    {
        public required int Id { get; init; }
        public required string Title { get; init; }
        public required string Content { get; init; }
        public required DateTime PublishedAt { get; init; }
        public string? PhotoURL { get; init; }
    }
}
