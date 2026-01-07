using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Create;
using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Delete_;
using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Update_;
using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.GetById;
using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.List;
namespace PawPal.API.Controllers.Animal_Info
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalHealthHistoryController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<int>> CreateAnimalHealthHistory
            (CreateAnimalHealthHistoryCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetAnimalHealthHistoryByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var healthHistory = await sender.Send(new GetAnimalHealthHistoryByIdQuery { Id = id }, ct);
            return healthHistory;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListAnimalHealthHistoriesQueryDto>> 
            List([FromQuery] ListAnimalHealthHistoriesQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }

        [HttpPut("{id:int}")]
        public async Task Update(int id, UpdateAnimalHealthHistoryCommand cmd, CancellationToken ct)
        {
            cmd.Id = id;
            await sender.Send(cmd, ct);
        }

        [HttpDelete("{id:int}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteAnimalHealthHistoryCommand { Id = id }, ct);
        }
    }
}
