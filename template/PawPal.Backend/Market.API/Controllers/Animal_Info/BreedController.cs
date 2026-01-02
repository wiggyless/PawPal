using PawPal.Application.Modules.Animal_Info.AnimalBreed.Commands.Create;
using PawPal.Application.Modules.Animal_Info.AnimalBreed.Commands.Delete;
using PawPal.Application.Modules.Animal_Info.AnimalBreed.Queries.GetById;
using PawPal.Application.Modules.Animal_Info.AnimalBreed.Queries.List;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Create;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Delete;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.GetById;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List;

namespace PawPal.API.Controllers.Animal_Info
{
    [ApiController]
    [Route("[controller]")]
    public class BreedController(ISender sender) : ControllerBase
    {
       
        [HttpPost]
        public async Task<ActionResult<int>> CreateAnimalCategory
            (CreateAnimalBreedCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id:int}")]
        public async Task<GetAnimalBreedByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var category = await sender.Send(new GetAnimalBreedByIdQuery { Id = id }, ct);
            return category;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListAnimalBreedQueryDto>> List([FromQuery] ListAnimalBreedQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }
        [HttpDelete("{id:int}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteAnimalBreedCommand { Id = id }, ct);
        }
    }
}
