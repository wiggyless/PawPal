
namespace PawPal.Application.Modules.News.Queries.List
{
    public sealed class ListNewsQuery : BasePagedQuery<ListNewsQueryDto>
    {
        public string? Search { get; init; }
    }

}
