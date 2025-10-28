using PawPal.Application.Abstractions;
namespace PawPal.Application.Modules.Places.Queries.GetById
{
    public class GetCityByIdQueryHandler(IAppDbContext context) : 
        IRequestHandler<GetCityByIdQuery,GetCityByIdQueryDto>
    {
        // ovo valjda izvlaci stvari koje trebaju, samo kopiram pa daj Boze da radi
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
                // ovde su oni pravili svoj exception, al ja cu ovaj klasik jer 
                // sta ce mi drugi :D
                throw new Exception("Nije nadjen grad");
            }
            return category;
        }
    }
}
