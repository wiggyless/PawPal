using PawPal.Application.Abstractions;

namespace PawPal.Application.Modules.Catalog.ProductCategories.Commands.Status.Disable;

public sealed class DisableProductCategoryCommandHandler(IAppDbContext ctx, IAppCurrentUser currentUser)
    : IRequestHandler<DisableProductCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DisableProductCategoryCommand request, CancellationToken ct)
    {
        if (currentUser.RoleId != 3)
            throw new PawPalConflictException("Only administrators can disable categories.");

        var cat = await ctx.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (cat is null)
        {
            throw new PawPalNotFoundException($"Category (ID={request.Id}) not found.");
        }

        if (!cat.IsEnabled) return Unit.Value; // idempotent

        // Business rule: cannot disable if there are active products
        var hasActiveProducts = await ctx.Products
            .AnyAsync(p => p.CategoryId == cat.Id && p.IsEnabled, ct);

        if (hasActiveProducts)
        {
            throw new MarketBusinessRuleException("category.disable.blocked.activeProducts",
                $"Category {cat.Name} cannot be disabled because it contains active products.");
        }

        cat.IsEnabled = false;

        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}