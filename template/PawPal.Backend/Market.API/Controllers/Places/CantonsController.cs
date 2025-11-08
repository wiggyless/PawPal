using PawPal.Application.Modules.Places.Cantons.Lists;
using PawPal.Application.Modules.Places.Cantons.Queries_;
using PawPal.Application.Modules.Places.Cities.Queries.GetById;

namespace PawPal.API.Controllers.Places
{
    [ApiController]
    [Route("[controller]")]
    public class CantonsController(ISender sender) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<GetCantonByIdQueryDto> GetById(int id,CancellationToken tk)
        {
            var category = await sender.Send(new GetCantonByIdQuery { Id = id }, tk);
            return category;
        }
        [HttpGet]

        public async Task<PageResult<ListCantonsQueryDto>> List([FromQuery] 
        ListCantonsQuery query,CancellationToken tk)
        {
            var result = await sender.Send(query, tk);
            return result;
        }
    }
}
