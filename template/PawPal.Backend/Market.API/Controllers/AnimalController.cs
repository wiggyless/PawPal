using PawPal.Application.Modules.Animal_Info.Animals.Commands.Create;
using PawPal.Application.Modules.Animal_Info.Animals.Queries.GetById;
using PawPal.Application.Modules.Animal_Info.Animals.Queries.List;

namespace PawPal.API.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class AnimalController(ISender sender) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<GetAnimalByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var animal = await sender.Send(new GetAnimalByIdQuery { Id = id }, ct);
            return animal;
        }

        [HttpGet]
        public async Task<PageResult<ListAnimalsQueryDto>> List([FromQuery] 
        ListAnimalsQuery query, CancellationToken ct)
        {
            var res = await sender.Send(query, ct);
            return res;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateAnimal(CreateAnimalCommand cmd, CancellationToken ct)
        {
            int id = await sender.Send(cmd, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
    }
}
