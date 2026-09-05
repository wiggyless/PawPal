
namespace PawPal.Application.Modules.News.Queries.List
{
    public sealed class ListNewsQuery : BasePagedQuery<ListNewsQueryDto>
    {
        public string? Search { get; init; }
        public DateTime? PublishedFrom { get; init; }
        public DateTime? PublishedTo { get; init; }
        public bool? HasPhoto { get; init; }
        public bool SortDescending { get; init; } = true;
    }

}
