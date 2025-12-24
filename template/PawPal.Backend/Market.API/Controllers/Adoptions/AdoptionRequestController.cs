using PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Create;
using PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Delete;
using PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.GetById;
using PawPal.Application.Modules.Adoptions.AdoptionRequests.Queries.List;
using PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Create;
using PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.GetById;
using PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.List;

namespace PawPal.API.Controllers.Adoptions
{
    [ApiController]
    [Route("[controller]")]
    public class AdoptionRequestController(ISender sender) : ControllerBase
    {
        [HttpPost]  
        public async Task<ActionResult<int>> CreateRequest(CreateAdoptionRequestCommand crc,CancellationToken cancellationToken)
        {
            int id = await sender.Send(crc, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id:int}")]
        public async Task<GetAdoptionRequestByIdQueryDto> GetById(int id, CancellationToken cancellationToken)
        {
            var req = await sender.Send(new GetAdoptionRequestByIdQuery { Id = id }, cancellationToken);
            return req;
        }

        [HttpGet]
        public async Task<PageResult<ListAdoptionRequestQueryDto>> List([FromQuery] ListAdoptionRequestQuery query,
            CancellationToken cancellationToken)
        {
            var res = await sender.Send(query, cancellationToken);
            return res;
        }
        [HttpDelete("{id:int}")]
        public async Task Delete(int id,CancellationToken cancellationToken)
        {
            await sender.Send(new DeleteAdoptionRequestCommand { Id = id }, cancellationToken);
        }
    }
}
