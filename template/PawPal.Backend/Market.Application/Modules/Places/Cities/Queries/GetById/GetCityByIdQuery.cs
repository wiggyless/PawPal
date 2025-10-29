namespace PawPal.Application.Modules.Places.Cities.Queries.GetById
{
    public class GetCityByIdQuery : IRequest<GetCityByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
