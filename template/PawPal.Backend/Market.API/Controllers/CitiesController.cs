﻿using PawPal.Application.Modules.Places.Cities.Queries.GetById;
using PawPal.Application.Modules.Places.Cities.Queries.Lists;


namespace PawPal.API.Controllers
{
[ApiController]
[Route("[controller]")]
    public class CitiesController(ISender sender) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<GetCityByIdQueryDto> GetById(int id,CancellationToken tk)
        {
            var category = await sender.Send(new GetCityByIdQuery { Id = id }, tk);
            return category;
        }

        [HttpGet]
        public async Task<PageResult<ListCitiesQueryDto>> List([FromQuery] 
        ListCitiesQuery query,CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }
    }
    
}
