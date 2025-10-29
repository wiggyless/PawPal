using PawPal.Application.Abstractions;
namespace PawPal.Application.Modules.Places.Cities.Queries.GetById
{
    public class GetCityByIdQueryHandler(IAppDbContext context) : 
        IRequestHandler<GetCityByIdQuery,GetCityByIdQueryDto>
    {
        
        // The way is lit, the path is clear, we require only the strenght to follow it
        public async Task<GetCityByIdQueryDto> Handle(GetCityByIdQuery request,CancellationToken toke)
        {
            var category = await context.Cities.
                Where(c => c.Id == request.Id).
                Select(x => new GetCityByIdQueryDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).FirstOrDefaultAsync(toke);
            if(category == null)
            {
              
                throw new PawPalNotFoundException("City not found");
            }
            return category;
        }
    }
}
