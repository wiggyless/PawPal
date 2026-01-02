using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List;
using PawPal.Application.Modules.Gender.List;

namespace PawPal.API.Controllers.Animal_Info
{
    [ApiController]
    [Route("[controller]")]
    public class GenderController(ISender sender): ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListGenderQueryDto>> List([FromQuery] ListGenderQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }
    }
}
