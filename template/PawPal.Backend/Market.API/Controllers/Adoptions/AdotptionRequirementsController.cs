using Microsoft.EntityFrameworkCore.Diagnostics;
using PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Create;
using PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Delete;
using PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Update;
using PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.GetById;
using PawPal.Application.Modules.Adoptions.AdoptionRequirements.Queries.List;

namespace PawPal.API.Controllers.Adoptions
{
    [ApiController]
    [Route("[controller]")]
    public class AdotptionRequirementsController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreateRequirement(CreateRequirementCommand crc, CancellationToken cancellationToken)
        {
            int id = await sender.Send(crc, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        [HttpGet("{id:int}")]
        public async Task<GetRequirementByIdQueryDto> GetById(int id,CancellationToken cancellationToken)
        {
            var req = await sender.Send(new GetRequirementByIdQuery { Id = id }, cancellationToken);
            return req;
        }

        [HttpGet]
        public async Task<PageResult<ListRequirementsQueryDto>> List([FromQuery] ListRequirementsQuery query,
            CancellationToken cancellationToken)
        {
            var res = await sender.Send(query, cancellationToken);
            return res;
        }
        [HttpPut("{id:int}")]
        public async Task Update(UpdateRequirementCommand crc,int id,CancellationToken cancellationToken)
        {
            crc.Id = id;
            await sender.Send(crc, cancellationToken);
        }
        [HttpDelete("{id:int}")]
        public async Task Delete(int id,CancellationToken cancellationToken)
        {
            await sender.Send(new DeleteRequirementCommand { Id = id }, cancellationToken);
        }
    }
}
