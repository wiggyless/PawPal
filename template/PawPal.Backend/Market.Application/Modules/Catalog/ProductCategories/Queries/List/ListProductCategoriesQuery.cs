using PawPal.Application.Common;

namespace PawPal.Application.Modules.Catalog.ProductCategories.Queries.List;

public sealed class ListProductCategoriesQuery : BasePagedQuery<ListProductCategoriesQueryDto>
{
    public string? Search { get; init; }
    public bool? OnlyEnabled { get; init; }
}
