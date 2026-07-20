using PawPal.Application.Abstractions;

namespace PawPal.Application.Modules.Catalog.ProductCategories.Commands.Delete;

public class DeleteProductCategoryCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
      : IRequestHandler<DeleteProductCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        if (appCurrentUser.RoleId != 3)
            throw new MarketBusinessRuleException("123", "Only administrators can delete categories.");

        var category = await context.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (category is null)
            throw new PawPalNotFoundException("Category not found.");

        category.IsDeleted = true; // Soft delete
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
