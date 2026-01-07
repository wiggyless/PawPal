using PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.GetById;
using PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.List;
using PawPal.Application.Modules.Allergies.Queries.GetById;
using PawPal.Application.Modules.Allergies.Queries.List;

namespace PawPal.API.Controllers.Allergies
{
    [ApiController]
    [Route("[controller]")]
    public class AllergiesController(ISender sender): ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetAllergyByIdDto> GetById(int id, CancellationToken cancellationToken)
        {
            var req = await sender.Send(new GetAllergyById { Id = id }, cancellationToken);
            return req;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListAllergyQueryDto>> List([FromQuery] ListAllergyQuery query,
            CancellationToken cancellationToken)
        {
            var res = await sender.Send(query, cancellationToken);
            return res;
        }
    }
}
