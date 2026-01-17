using PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.GetById;
using PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.List;
using PawPal.Application.Modules.Allergies.Queries.GetById;
using PawPal.Application.Modules.Allergies.Queries.List;
using PawPal.Application.Modules.Disabilities.GetById;
using PawPal.Application.Modules.Disabilities.List;

namespace PawPal.API.Controllers.Allergies
{
    [ApiController]
    [Route("[controller]")]
    public class DisabilitiesController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetDisabilityByIdDto> GetById(int id, CancellationToken cancellationToken)
        {
            var req = await sender.Send(new GetDisabilityById { Id = id }, cancellationToken);
            return req;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListDisabilityQueryDto>> List([FromQuery] ListDisabilityQuery query,
            CancellationToken cancellationToken)
        {
            var res = await sender.Send(query, cancellationToken);
            return res;
        }
    }
}
