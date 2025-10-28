using PawPal.Application.Modules.Animal_Info.Animals.Commands.Create;
using PawPal.Application.Modules.Animal_Info.Animals.Queries.GetById;

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

        [HttpPost]
        public async Task<ActionResult<int>> CreateAnimal(CreateAnimalCommand cmd, CancellationToken ct)
        {
            int id = await sender.Send(cmd, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
    }
}
