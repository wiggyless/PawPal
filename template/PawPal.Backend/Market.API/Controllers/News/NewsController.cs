using PawPal.Application.Modules.News.Commands.Create;
using PawPal.Application.Modules.News.Commands.Delete;
using PawPal.Application.Modules.News.Commands.Update;
using PawPal.Application.Modules.News.Queries.GetById;
using PawPal.Application.Modules.News.Queries.List;

namespace PawPal.API.Controllers.News
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreateNews
            ([FromForm] CreateNewsCommand command, CancellationToken ct) //from form means we will be handling file uploads
        {
            int id = await sender.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetNewsByIdQueryDto>> GetById(int id, CancellationToken ct)
        {
            var news = await sender.Send(new GetNewsByIdQuery { Id = id }, ct);
            return news;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListNewsQueryDto>> List([FromQuery]
        ListNewsQuery query, CancellationToken ct)
        {
            var res = await sender.Send(query, ct);
            return res;
        }

        [HttpDelete("{id:int}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteNewsCommand { Id = id }, ct);
        }

        [HttpPut("{id:int}")]
        public async Task Update(int id, [FromForm] UpdateNewsCommand command, CancellationToken ct)
        {
            command.Id = id;
            await sender.Send(command, ct);
        }
    }
}
