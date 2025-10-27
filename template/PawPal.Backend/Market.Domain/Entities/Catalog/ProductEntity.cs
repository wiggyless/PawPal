﻿using PawPal.Domain.Common;

namespace PawPal.Domain.Entities.Catalog;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class ProductEntity : BaseEntity
{
    /// <summary>
    /// Name of the product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Product description. (optional)
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity of the product available in stock.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Identifier of the category to which the product belongs.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Navigation reference to the product's category.
    /// </summary>
    public ProductCategoryEntity? Category { get; set; }

    /// <summary>
    /// IsEnabled
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Single source of truth for technical/business constraints.
    /// Used in validators and EF configuration.
    /// </summary>
    public static class Constraints
    {
        public const int NameMaxLength = 150;

        public const int DescriptionMaxLength = 1000;
    }
}