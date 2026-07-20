using PawPal.Application.Abstractions;

namespace PawPal.Application.Modules.Catalog.ProductCategories.Commands.Update;

public sealed class UpdateProductCategoryCommandHandler(IAppDbContext ctx, IAppCurrentUser currentUser)
            : IRequestHandler<UpdateProductCategoryCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProductCategoryCommand request, CancellationToken ct)
    {
        if (currentUser.RoleId != 3)
            throw new PawPalConflictException("Only administrators can update categories.");

        var entity = await ctx.ProductCategories
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(ct);

        if (entity is null)
            throw new PawPalNotFoundException($"Category (ID={request.Id}) not found.");

        // Check for duplicate name (case-insensitive, except for the same ID)
        var exists = await ctx.ProductCategories
            .AnyAsync(x => x.Id != request.Id && x.Name.ToLower() == request.Name.ToLower(), ct);

        if (exists)
        {
            throw new PawPalConflictException("Name already exists.");
        }

        entity.Name = request.Name.Trim();

        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}