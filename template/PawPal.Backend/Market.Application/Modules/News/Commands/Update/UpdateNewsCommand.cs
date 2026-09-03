using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.News.Commands.Update
{
    public sealed class UpdateNewsCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
