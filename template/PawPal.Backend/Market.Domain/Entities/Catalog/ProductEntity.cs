using PawPal.Domain.Common;

namespace PawPal.Domain.Entities.Catalog;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class ProductEntity : BaseEntity
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }


    public int CategoryId { get; set; }


    public ProductCategoryEntity? Category { get; set; }
    public bool IsEnabled { get; set; }

    public static class Constraints
    {
        public const int NameMaxLength = 150;

        public const int DescriptionMaxLength = 1000;
    }
}