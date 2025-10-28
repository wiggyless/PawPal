
namespace PawPal.Application.Modules.Places.Queries.GetById
{
    public class GetCityByIdQuery : IRequest<GetCityByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
