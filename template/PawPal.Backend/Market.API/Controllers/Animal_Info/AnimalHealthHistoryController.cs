
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List;
using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Create;
using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.GetById;
using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Queries.List;
namespace PawPal.API.Controllers.Animal_Info
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalHealthHistoryController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreateAnimalHealthHistory
            (CreateAnimalHealthHistoryCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id:int}")]
        public async Task<GetAnimalHealthHistoryByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var healthHistory = await sender.Send(new GetAnimalHealthHistoryByIdQuery { Id = id }, ct);
            return healthHistory;
        }

        [HttpGet]
        public async Task<PageResult<ListAnimalHealthHistoriesQueryDto>> 
            List([FromQuery] ListAnimalHealthHistoriesQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }
    }
}
