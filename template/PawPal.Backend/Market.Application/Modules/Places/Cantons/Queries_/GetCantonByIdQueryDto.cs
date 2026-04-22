

namespace PawPal.Application.Modules.Places.Cantons.Queries_
{
    public class GetCantonByIdQueryDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required List<CantonCitiesID> Cities { get; set; }
    }
    public sealed class CantonCitiesID
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
