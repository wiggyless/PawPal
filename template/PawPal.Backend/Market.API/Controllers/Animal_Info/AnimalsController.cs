using PawPal.Application.Modules.Animal_Info.Animals.Commands.Create;
using PawPal.Application.Modules.Animal_Info.Animals.Commands.Delete;
using PawPal.Application.Modules.Animal_Info.Animals.Commands.Update;
using PawPal.Application.Modules.Animal_Info.Animals.Queries.GetById;
using PawPal.Application.Modules.Animal_Info.Animals.Queries.List;

namespace PawPal.API.Controllers.Animal_Info
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalsController(ISender sender) : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<int>> CreateAnimal(CreateAnimalCommand cmd, CancellationToken ct)
        {
            int id = await sender.Send(cmd, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetAnimalByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var animal = await sender.Send(new GetAnimalByIdQuery { Id = id }, ct);
            return animal;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListAnimalsQueryDto>> List([FromQuery]
        ListAnimalsQuery query, CancellationToken ct)
        {
            var res = await sender.Send(query, ct);
            return res;
        }

        [HttpPut("{id:int}")]
        public async Task Update(UpdateAnimalCommand cmd, int id, CancellationToken ct)
        {
            cmd.Id = id;
            await sender.Send(cmd, ct);
        }


        [HttpDelete("{id:int}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteAnimalCommand { Id = id }, ct);
        }
    }
}
