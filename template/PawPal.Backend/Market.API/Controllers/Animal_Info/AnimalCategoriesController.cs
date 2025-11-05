using PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Create;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Delete;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.GetById;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List;

namespace PawPal.API.Controllers.Animal_Info
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalCategoriesController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreateAnimalCategory
            (CreateAnimalCategoryCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id:int}")]
        public async Task<GetAnimalCategoryByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var category = await sender.Send(new GetAnimalCategoryByIdQuery { Id = id }, ct);
            return category;
        }

        [HttpGet]
        public async Task<PageResult<ListAnimalCategoriesQueryDto>> List([FromQuery] ListAnimalCategoriesQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }

        [HttpPut ("{id:int}")]
        public async Task Update(int id, UpdateAnimalCategoryCommand cmd, CancellationToken ct)
        {
            cmd.Id = id;
            await sender.Send(cmd, ct);
        }


        [HttpDelete("{id:int}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteAnimalCategoryCommand { Id = id }, ct);
        }


    }


}
