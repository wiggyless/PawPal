using PawPal.Domain.Common;

namespace PawPal.Domain.Entities.Catalog;


public class ProductCategoryEntity : BaseEntity
{

    public string Name { get; set; }


    public bool IsEnabled { get; set; }

    public IReadOnlyCollection<ProductEntity> Products { get; private set; } = new List<ProductEntity>();

    public static class Constraints
    {
        public const int NameMaxLength = 100;
    }
}