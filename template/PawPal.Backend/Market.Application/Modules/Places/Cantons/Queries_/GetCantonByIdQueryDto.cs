

namespace PawPal.Application.Modules.Places.Cantons.Queries_
{
    public class GetCantonByIdQueryDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
    }
}
